using BrixCMS.Open.Data;
using BrixCMS.Open.Extensions;
using BrixCMS.Open.Services;
using BrixCMS.Open.Services.Email;
using BrixCMS.Open.Services.Ingestion;

using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.InMemory;
using OllamaSharp;

using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI;
using System.ClientModel;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// 0️⃣ SECURITY — antiforgery header, rate limiting, size limits
// =====================================================
builder.Services.AddAntiforgery(opts => opts.HeaderName = "X-CSRF-TOKEN");

builder.Services.AddHttpClient();

builder.Services.AddRateLimiter(opts =>
{
    opts.AddFixedWindowLimiter("login", o =>
    {
        o.PermitLimit = 5;
        o.Window = TimeSpan.FromMinutes(1);
        o.QueueLimit = 0;
        o.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
    opts.AddFixedWindowLimiter("ai", o =>
    {
        o.PermitLimit = 20;
        o.Window = TimeSpan.FromMinutes(1);
        o.QueueLimit = 0;
        o.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
    opts.RejectionStatusCode = 429;
    opts.OnRejected = async (ctx, _) =>
    {
        ctx.HttpContext.Response.ContentType = "text/html";
        await ctx.HttpContext.Response.WriteAsync(
            "<h2 style='font-family:sans-serif;margin:4rem auto;text-align:center'>Too many login attempts. Please wait 1 minute.</h2>");
    };
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o =>
    o.MultipartBodyLengthLimit = 52_428_800); // 50 MB
builder.WebHost.ConfigureKestrel(o =>
    o.Limits.MaxRequestBodySize = 52_428_800);

// =====================================================
// 1️⃣ DATABASE
// =====================================================
builder.Services.AddDbContext<BrixDbContext>(options =>
    options.UseSqlite("Data Source=brix.db"));

// =====================================================
// 2️⃣ MVC + CMS SERVICES
// =====================================================
builder.Services.AddControllersWithViews();
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache(o => o.SizeLimit = 256 * 1024 * 1024); // 256 MB cap for image cache
builder.Services.AddScoped<ContentService>();
builder.Services.AddScoped<AdminAuthService>();
builder.Services.AddbrixBlocks();
builder.Services.AddTransient<EmailSender>();

builder.Services.AddScoped<ApiKeyService>();
builder.Services.AddSingleton<PromptsService>();

// =====================================================
// 2️⃣ SESSION
// =====================================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Name = ".BrixCMS.Open.Session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

// =====================================================
// 3️⃣ BLAZOR
// =====================================================
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options => options.DetailedErrors = true);

// =====================================================
// 4️⃣ MARKDOWN
// =====================================================
builder.Services.AddSingleton<MarkdownPipeline>(_ =>
    new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseEmojiAndSmiley()
        .UseSoftlineBreakAsHardlineBreak()
        .Build());

// =====================================================
// 5️⃣ AI — Ollama (local, free, open source)
// =====================================================

var ollamaUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
var chatModel = builder.Configuration["Ollama:ChatModel"] ?? "llama3.1:8b";
var embeddingModel = builder.Configuration["Ollama:EmbeddingModel"]!;

// Embeddings always use Ollama (needed for PDF semantic search)
var embeddingGenerator = new OllamaApiClient(new Uri(ollamaUrl), embeddingModel);

// =====================================================
// VECTOR STORE (InMemory — SK connector + VectorData 10.1.0)
// Los datos se reingresan al arranque via la lógica de ingesta existente.
// =====================================================
builder.Services.AddInMemoryVectorStore();
builder.Services.AddInMemoryVectorStoreRecordCollection<string, IngestedChunk>("data-chatappollama-chunks");
builder.Services.AddInMemoryVectorStoreRecordCollection<string, IngestedDocument>("data-chatappollama-documents");

// =====================================================
// EMBEDDINGS
// =====================================================

builder.Services.AddEmbeddingGenerator(embeddingGenerator);

// =====================================================
// CHAT CLIENT
// =====================================================

builder.Services.AddScoped<IChatClient>(sp =>
{
    var apiKeySvc = sp.GetRequiredService<ApiKeyService>();
    var loggerFac = sp.GetRequiredService<ILoggerFactory>();
    var resolved  = apiKeySvc.ResolveActiveClient();
    var endpoint  = resolved?.endpoint ?? ollamaUrl;
    var model     = resolved?.model    ?? chatModel;

    return ((IChatClient)new OllamaApiClient(new Uri(endpoint), model))
        .AsBuilder()
        .UseFunctionInvocation()
        .UseLogging(loggerFac)
        .Build();
});

// =====================================================
// 7️⃣ INGESTION AND SEARCH SERVICES
// =====================================================
builder.Services.AddScoped<DataIngestor>();
builder.Services.AddSingleton<SemanticSearch>();
builder.Services.AddTransient<EmailSender>();

// =====================================================
// 8️⃣ HTTP PIPELINE
// =====================================================
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseResponseCaching();
app.UseAntiforgery();
app.UseSession();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
    ctx.Response.Headers["X-Frame-Options"] = "DENY";
    ctx.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    ctx.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    ctx.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    await next();
});

// =====================================================
// 9️⃣ ROUTING - Order matters! More specific first
// =====================================================

// 0. Admin login at friendly URL /admin/manager
app.MapControllerRoute(
    name: "admin-login",
    pattern: "admin/manager",
    defaults: new { area = "Manager", controller = "Login", action = "Index" });

// 1. Manager area (important to come first)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Manager}/{action=Index}/{id?}");

// 4. BrixCMS Landing page (marketing / sales)
app.MapControllerRoute(
    name: "landing",
    pattern: "landing",
    defaults: new { controller = "Landing", action = "Index" });

// 5. Blazor Web App (BEFORE cms catch-all so _framework/ works)
app.MapRazorComponents<BrixCMS.Open.Components.App>()
    .AddInteractiveServerRenderMode();

// 6. CMS catch-all (last) — exclude _framework, api, _content paths
app.MapControllerRoute(
    name: "cms",
    pattern: "{slug?}",
    defaults: new { controller = "Cms", action = "Index" },
    constraints: new { slug = @"^(?!_framework|api|_content|\.well-known).*$" });

// =====================================================
// 🔟 INITIALIZATION — DB + PDF INGESTION
// =====================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BrixDbContext>();
    db.Database.EnsureCreated();

    // Manual migration: PageViews table
    try
    {
        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""PageViews"" (
                ""Id""        INTEGER PRIMARY KEY AUTOINCREMENT,
                ""ViewedAt""  TEXT NOT NULL,
                ""Slug""      TEXT NOT NULL,
                ""UserAgent"" TEXT,
                ""Referrer""  TEXT
            )");
    }
    catch { }

    // Manual migration: SEO columns on Pages table
    var newPageColumns = new Dictionary<string, string>
    {
        ["MetaDescription"] = "TEXT",
        ["OgImage"] = "TEXT",
        ["MetaKeywords"] = "TEXT",
        ["IsSeed"] = "INTEGER NOT NULL DEFAULT 0",
    };
    foreach (var col in newPageColumns)
    {
        try { db.Database.ExecuteSqlRaw($"ALTER TABLE Pages ADD COLUMN \"{col.Key}\" {col.Value}"); }
        catch { }
    }
    // Mark the existing demo page as seed (applies to DBs created before this column existed)
    try { db.Database.ExecuteSqlRaw("UPDATE Pages SET IsSeed = 1 WHERE Title = 'Home' AND IsSeed = 0 AND (SELECT COUNT(*) FROM Pages WHERE IsSeed = 0 AND IsPublished = 1) <= 1"); }
    catch { }

    // ── Seed BrixCMS landing page (first run) ──
    BrixCMS.Open.Services.BrixLandingSeeder.SeedIfEmpty(db);

    // Manual migration: Subscribers table
    try
    {
        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""Subscribers"" (
                ""Id""        INTEGER PRIMARY KEY AUTOINCREMENT,
                ""Email""     TEXT NOT NULL UNIQUE,
                ""Name""      TEXT,
                ""CreatedAt"" TEXT NOT NULL DEFAULT (datetime('now'))
            )");
    }
    catch { /* already exists */ }

    // Manual migration: AdminUser new columns (safe if already exist)
    var adminNewColumns = new Dictionary<string, string>
    {
        ["Name"]      = "TEXT NOT NULL DEFAULT ''",
        ["IsOwner"]   = "INTEGER NOT NULL DEFAULT 0",
        ["CreatedAt"] = "TEXT NOT NULL DEFAULT '2024-01-01T00:00:00'"
    };
    foreach (var col in adminNewColumns)
    {
        try { db.Database.ExecuteSqlRaw($"ALTER TABLE AdminUsers ADD COLUMN \"{col.Key}\" {col.Value}"); }
        catch { }
    }
    // Ensure the first admin is always owner
    try { db.Database.ExecuteSqlRaw("UPDATE AdminUsers SET IsOwner = 1 WHERE Id = (SELECT MIN(Id) FROM AdminUsers)"); }
    catch { }

    // ── Seed admin on first run ──
    if (!db.AdminUsers.Any())
    {
        var hash = BrixCMS.Open.Services.AdminAuthService.HashPassword("admin123", out var salt);
        db.AdminUsers.Add(new BrixCMS.Open.Data.AdminUser
        {
            Email        = "admin@brix.com",
            Name         = "Owner",
            PasswordHash = hash,
            PasswordSalt = salt,
            IsOwner      = true,
            CreatedAt    = DateTime.UtcNow
        });
        db.SaveChanges();

        var startupLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        startupLogger.LogWarning("==========================================================");
        startupLogger.LogWarning("ADMIN CREATED — email: admin@brix.com  password: admin123");
        startupLogger.LogWarning("Change this immediately at /Manager/Login after first login.");
        startupLogger.LogWarning("==========================================================");
    }

    var dataPath = Path.Combine(builder.Environment.WebRootPath ?? "wwwroot", "Data");

    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("📁 Looking for PDFs in: {path}", dataPath);
    logger.LogInformation("📁 Folder exists: {exists}", Directory.Exists(dataPath));

    if (Directory.Exists(dataPath))
    {
        var pdfs = Directory.GetFiles(dataPath, "*.pdf");
        logger.LogInformation("📄 PDFs found: {count}", pdfs.Length);
        foreach (var pdf in pdfs)
            logger.LogInformation("📄 File: {file}", pdf);

        // ✅ FIX: Get the generator from DI to pass to the source
        var generator = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

        await DataIngestor.IngestDataAsync(
            app.Services,
            new PDFDirectorySource(dataPath, generator) // <-- Now passing the generator
        );
    }
    else
    {
        logger.LogWarning("⚠️ The Data folder does not exist at: {path}", dataPath);
    }
}

// =====================================================
// 🔍 DEBUG ENDPOINT (development only)
// =====================================================
if (app.Environment.IsDevelopment())
{
    app.MapPost("/api/debug-chat", async (IChatClient chat, [FromBody] string prompt) =>
    {
        try
        {
            var response = await chat.GetResponseAsync(prompt);
            return Results.Ok(new { ok = true, text = response.Text });
        }
        catch (Exception ex)
        {
            return Results.Ok(new { ok = false, error = ex.Message, type = ex.GetType().Name });
        }
    });
}

app.Run();

