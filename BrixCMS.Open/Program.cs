using BrixCMS.Open.Data;
using BrixCMS.Open.Extensions;
using BrixCMS.Open.Services;
using BrixCMS.Open.Services.Email;
using BrixCMS.Open.Services.Ingestion;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using OpenAI;
using OllamaSharp;
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
    opts.AddFixedWindowLimiter("checkout", o =>
    {
        o.PermitLimit = 10;
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

// AI key management
builder.Services.AddSingleton<EncryptionService>();
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
    options.Cookie.Name = ".BrixCMS.Open.Session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.IdleTimeout = TimeSpan.FromHours(24);
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
// 5️⃣ AI — Dynamic provider (Gemini / DeepSeek / Mistral) with Ollama fallback
// =====================================================
var ollamaUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
var chatModel = builder.Configuration["Ollama:ChatModel"] ?? "llama3.1:8b";
var embeddingModel = builder.Configuration["Ollama:EmbeddingModel"]!;

// Embeddings always use Ollama (needed for PDF semantic search)
var embeddingGenerator = new OllamaApiClient(new Uri(ollamaUrl), embeddingModel);

var vectorStorePath = Path.Combine(AppContext.BaseDirectory, "vector-store.db");
var vectorStoreConnectionString = $"Data Source={vectorStorePath}";

builder.Services.AddSqliteCollection<string, IngestedChunk>("data-chatappollama-chunks", vectorStoreConnectionString);
builder.Services.AddSqliteCollection<string, IngestedDocument>("data-chatappollama-documents", vectorStoreConnectionString);

builder.Services.AddEmbeddingGenerator(embeddingGenerator);

// IChatClient: checks DB for a configured external provider; falls back to Ollama
builder.Services.AddScoped<IChatClient>(sp =>
{
    var apiKeySvc = sp.GetRequiredService<ApiKeyService>();
    var loggerFac = sp.GetRequiredService<ILoggerFactory>();
    var resolved = apiKeySvc.ResolveActiveClient();

    if (resolved.HasValue)
    {
        var (provider, endpoint, model) = resolved.Value;

        // Ollama: no API key needed
        if (provider == "ollama")
        {
            return ((IChatClient)new OllamaApiClient(new Uri(endpoint), model))
                .AsBuilder()
                .UseFunctionInvocation()
                .UseLogging(loggerFac)
                .Build();
        }

        var plainKey = apiKeySvc.GetDecryptedKeySync(provider)!;
        var openAiClient = new OpenAIClient(
            new ApiKeyCredential(plainKey),
            new OpenAIClientOptions { Endpoint = new Uri(endpoint) });

        return openAiClient
            .GetChatClient(model)
            .AsIChatClient()
            .AsBuilder()
            .UseFunctionInvocation()
            .UseLogging(loggerFac)
            .Build();
    }

    // Fallback: Ollama (appsettings / default)
    return ((IChatClient)new OllamaApiClient(new Uri(ollamaUrl), chatModel))
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

// 2. Cart routes
app.MapControllerRoute(
    name: "cart",
    pattern: "Cart/{action=Index}/{id?}",
    defaults: new { controller = "Cart", action = "Index" });

// 4. BrixCMS Landing page (marketing / sales)
app.MapControllerRoute(
    name: "landing",
    pattern: "landing",
    defaults: new { controller = "Landing", action = "Index" });

// 5. CMS catch-all (last)
app.MapControllerRoute(
    name: "cms",
    pattern: "{slug?}",
    defaults: new { controller = "Cms", action = "Index" });

app.MapRazorComponents<BrixCMS.Open.Components.App>()
    .AddInteractiveServerRenderMode();

// =====================================================
// 🔟 INITIALIZATION — DB + PDF INGESTION
// =====================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BrixDbContext>();
    db.Database.EnsureCreated();

    // Manual migration: ApiKeys table (safe if already exists)
    try
    {
        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""ApiKeys"" (
                ""Id""           INTEGER PRIMARY KEY AUTOINCREMENT,
                ""Provider""     TEXT NOT NULL UNIQUE,
                ""EncryptedKey"" TEXT NOT NULL,
                ""Iv""           TEXT NOT NULL,
                ""AuthTag""      TEXT NOT NULL,
                ""CreatedAt""    TEXT NOT NULL,
                ""UpdatedAt""    TEXT NOT NULL
            )");
    }
    catch { /* already exists */ }

    // Manual migration: AiUsageLogs table
    try
    {
        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""AiUsageLogs"" (
                ""Id""               INTEGER PRIMARY KEY AUTOINCREMENT,
                ""CreatedAt""        TEXT NOT NULL,
                ""Operation""        TEXT NOT NULL,
                ""Provider""         TEXT NOT NULL,
                ""Model""            TEXT NOT NULL,
                ""InputTokens""      INTEGER NOT NULL DEFAULT 0,
                ""OutputTokens""     INTEGER NOT NULL DEFAULT 0,
                ""EstimatedCostUsd"" TEXT NOT NULL DEFAULT '0'
            )");
    }
    catch { /* already exists */ }

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

    // Manual migration: AiGenerationLogs table
    try
    {
        db.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""AiGenerationLogs"" (
                ""Id""         INTEGER PRIMARY KEY AUTOINCREMENT,
                ""CreatedAt""  TEXT NOT NULL,
                ""PageId""     TEXT,
                ""PageTitle""  TEXT NOT NULL DEFAULT '',
                ""Prompt""     TEXT NOT NULL DEFAULT '',
                ""Provider""   TEXT NOT NULL DEFAULT '',
                ""Model""      TEXT NOT NULL DEFAULT '',
                ""Mode""       TEXT NOT NULL DEFAULT 'create'
            )");
    }
    catch { }

    // Manual migration: SEO columns on Pages table
    var newPageColumns = new Dictionary<string, string>
    {
        ["MetaDescription"] = "TEXT",
        ["OgImage"] = "TEXT",
        ["MetaKeywords"] = "TEXT",
    };
    foreach (var col in newPageColumns)
    {
        try { db.Database.ExecuteSqlRaw($"ALTER TABLE Pages ADD COLUMN \"{col.Key}\" {col.Value}"); }
        catch { }
    }

    // ── Seed BrixCMS landing page (marketing) ──
    BrixCMS.Open.Services.BrixLandingSeeder.SeedIfEmpty(db);

    // ── Seed Block Showcase demo page ──
    BrixCMS.Open.Services.BlockShowcaseSeeder.SeedIfEmpty(db);

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

    // ── Seed admin on first run with a random one-time password ──
    if (!db.AdminUsers.Any())
    {
        // Generate a cryptographically random 16-char password
        var rng = System.Security.Cryptography.RandomNumberGenerator.GetBytes(12);
        var oneTimePassword = Convert.ToBase64String(rng).Replace("=", "").Replace("/", "_").Replace("+", "-");
        var hash = BrixCMS.Open.Services.AdminAuthService.HashPassword(oneTimePassword, out var salt);
        db.AdminUsers.Add(new BrixCMS.Open.Data.AdminUser
        {
            Email = "admin@brix.com",
            PasswordHash = hash,
            PasswordSalt = salt
        });
        db.SaveChanges();

        var startupLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        startupLogger.LogWarning("==========================================================");
        startupLogger.LogWarning("ADMIN CREATED — one-time password: {Password}", oneTimePassword);
        startupLogger.LogWarning("Change this immediately at /admin/manager after first login.");
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




//using BrixCMS.Open.Data;
//using BrixCMS.Open.Extensions;
//using BrixCMS.Open.Services;
//using BrixCMS.Open.Services.Ingestion;
//using Markdig;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.AI;
//using OpenAI;
//using System.ClientModel;
//using BlazorApp = BrixCMS.Web.Components.App;

//var builder = WebApplication.CreateBuilder(args);

//// =====================================================
//// 1️⃣ BASE DE DATOS
//// =====================================================
//builder.Services.AddDbContext<brixDbContext>(options =>
//    options.UseSqlite("Data Source=brix.db"));

//// =====================================================
//// 2️⃣ MVC + SERVICIOS CMS
//// =====================================================
//builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<ContentService>();
//builder.Services.AddbrixBlocks();

//// =====================================================
//// 3️⃣ BLAZOR
//// =====================================================
//builder.Services
//    .AddRazorComponents()
//    .AddInteractiveServerComponents()
//    .AddCircuitOptions(options => options.DetailedErrors = true);

//// =====================================================
//// 4️⃣ MARKDOWN
//// =====================================================
//builder.Services.AddSingleton<MarkdownPipeline>(_ =>
//    new MarkdownPipelineBuilder()
//        .UseAdvancedExtensions()
//        .UseEmojiAndSmiley()
//        .UseSoftlineBreakAsHardlineBreak()
//        .Build());

//// =====================================================
//// 5️⃣ IA — MISTRAL VÍA OPENAI COMPATIBLE
//// =====================================================
//var mistralApiKey = builder.Configuration["Mistral:ApiKey"]
//    ?? throw new InvalidOperationException("Falta 'Mistral:ApiKey' en appsettings.json");

//var chatModel = builder.Configuration["Mistral:ChatModel"] ?? "mistral-small-latest";
//var embeddingModel = builder.Configuration["Mistral:EmbeddingModel"] ?? "mistral-embed";

//var openAIClient = new OpenAIClient(
//    new ApiKeyCredential(mistralApiKey),
//    new OpenAIClientOptions { Endpoint = new Uri("https://api.mistral.ai/v1") }
//);

//var chatClient = openAIClient
//    .GetChatClient(chatModel)
//    .AsIChatClient()
//    .AsBuilder()
//    .UseFunctionInvocation()
//    .Build();

//var embeddingClient = openAIClient
//    .GetEmbeddingClient(embeddingModel)
//    .AsIEmbeddingGenerator();

//builder.Services.AddChatClient(chatClient);
//builder.Services.AddEmbeddingGenerator(embeddingClient);

//// =====================================================
//// 6️⃣ VECTOR STORE (SQLiteVec)
//// =====================================================
//var vectorStorePath = Path.Combine(AppContext.BaseDirectory, "vector-store.db");
//var vectorStoreConnectionString = $"Data Source={vectorStorePath}";

//builder.Services.AddSqliteCollection<string, IngestedChunk>("data-chunks", vectorStoreConnectionString);
//builder.Services.AddSqliteCollection<string, IngestedDocument>("data-documents", vectorStoreConnectionString);

//// =====================================================
//// 7️⃣ SERVICIOS DE INGESTA Y BÚSQUEDA
//// =====================================================
//builder.Services.AddScoped<DataIngestor>();
//builder.Services.AddSingleton<SemanticSearch>();

//// =====================================================
//// 8️⃣ PIPELINE HTTP
//// =====================================================
//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//    app.UseDeveloperExceptionPage();
//else
//{
//    app.UseExceptionHandler("/Error", createScopeForErrors: true);
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAntiforgery();

//// =====================================================
//// 9️⃣ ROUTING
//// =====================================================
//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Manager}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "cms",
//    pattern: "{slug?}",
//    defaults: new { controller = "Cms", action = "Index" });

//app.MapRazorComponents<BlazorApp>()
//    .AddInteractiveServerRenderMode();

//// =====================================================
//// 🔟 INICIALIZACIÓN — BD + INGESTA DE PDFs
//// =====================================================
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<brixDbContext>();
//    db.Database.EnsureCreated();

//    var dataPath = Path.Combine(builder.Environment.WebRootPath ?? "wwwroot", "Data");

//    // ✅ Logs de diagnóstico
//    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//    logger.LogInformation("📁 Buscando PDFs en: {path}", dataPath);
//    logger.LogInformation("📁 Carpeta existe: {exists}", Directory.Exists(dataPath));

//    if (Directory.Exists(dataPath))
//    {
//        var pdfs = Directory.GetFiles(dataPath, "*.pdf");
//        logger.LogInformation("📄 PDFs encontrados: {count}", pdfs.Length);
//        foreach (var pdf in pdfs)
//            logger.LogInformation("📄 Archivo: {file}", pdf);

//        await DataIngestor.IngestDataAsync(
//            app.Services,
//            new PDFDirectorySource(dataPath)
//        );
//    }
//    else
//    {
//        logger.LogWarning("⚠️ La carpeta Data no existe en: {path}", dataPath);
//    }
//}

//// =====================================================
//// 🔍 DEBUG ENDPOINT (solo desarrollo)
//// =====================================================
//if (app.Environment.IsDevelopment())
//{
//    app.MapPost("/api/debug-chat", async (IChatClient chat, [FromBody] string prompt) =>
//    {
//        try
//        {
//            var response = await chat.GetResponseAsync(prompt);
//            return Results.Ok(new { ok = true, text = response.Text });
//        }
//        catch (Exception ex)
//        {
//            return Results.Ok(new { ok = false, error = ex.Message, type = ex.GetType().Name });
//        }
//    });
//}

//app.Run();
