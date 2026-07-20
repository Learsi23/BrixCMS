using BrixCMS.Open.Data;
using BrixCMS.Open.Extensions;
using BrixCMS.Open.Extensions.Authorization;
using BrixCMS.Open.Services;
using BrixCMS.Open.Services.Email;
using BrixCMS.Open.Services.Ingestion;

using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.InMemory;
using OllamaSharp;

using Markdig;
using Microsoft.AspNetCore.Authentication.Cookies;
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
// 1️⃣ DATABASE  — sqlite | sqlserver | postgres
// =====================================================
var dbProvider = (builder.Configuration["DatabaseProvider"] ?? "sqlite").ToLowerInvariant();

builder.Services.AddDbContext<BrixDbContext>(options =>
{
    switch (dbProvider)
    {
        case "sqlserver":
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("SqlServer")
                ?? throw new InvalidOperationException("ConnectionStrings:SqlServer is required when DatabaseProvider=sqlserver"));
            break;
        case "postgres":
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("ConnectionStrings:Postgres is required when DatabaseProvider=postgres"));
            break;
        default:
            options.UseSqlite("Data Source=brix.db");
            break;
    }
});

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

builder.Services.AddSingleton<EncryptionService>();
builder.Services.AddScoped<ApiKeyService>();
builder.Services.AddSingleton<PromptsService>();
builder.Services.AddScoped<IAiClientFactory, AiClientFactory>();

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
// 2️⃣a AUTHENTICATION & AUTHORIZATION — admin identity
// =====================================================
// Standard ASP.NET Core cookie auth replaces the old "Session['AdminAuth']=='1'" convention.
// AdminAuthService.BuildPrincipal() maps an AdminUser row to the ClaimsPrincipal signed in
// here; password hashing (PBKDF2) and TOTP verification are unchanged, only what happens
// after a successful check (SignInAsync instead of writing session strings) is new.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = ".BrixCMS.Open.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.LoginPath = "/admin/manager";
        options.AccessDeniedPath = "/Manager";
        options.Events.OnRedirectToLogin = ctx =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api") ||
                string.Equals(ctx.Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
            ctx.Response.Redirect(ctx.RedirectUri);
            return Task.CompletedTask;
        };
    });

builder.Services.AddAdminAuthorization();

// =====================================================
// 3️⃣ BLAZOR
// =====================================================
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options =>
    {
        if (builder.Environment.IsDevelopment())
            options.DetailedErrors = true;
    });

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

var embeddingModel = builder.Configuration["Ollama:EmbeddingModel"] ?? "all-minilm:latest";
var ollamaUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";

// Embeddings always use local Ollama (needed for PDF semantic search)
var embeddingGenerator = new OllamaApiClient(new Uri(ollamaUrl), embeddingModel);

// Vector store
builder.Services.AddInMemoryVectorStore();
builder.Services.AddInMemoryVectorStoreRecordCollection<string, IngestedChunk>("data-chatappollama-chunks");
builder.Services.AddInMemoryVectorStoreRecordCollection<string, IngestedDocument>("data-chatappollama-documents");

// Embeddings
builder.Services.AddEmbeddingGenerator(embeddingGenerator);

// Chat client — resolves provider dynamically via AiClientFactory
builder.Services.AddScoped<IChatClient>(sp =>
{
    var factory = sp.GetRequiredService<IAiClientFactory>();
    var loggerFac = sp.GetRequiredService<ILoggerFactory>();
    var (client, _, _) = factory.BuildJsonClient();
    return client
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
builder.Services.AddScoped<SiteSettingsImporter>();
builder.Services.AddTransient<EmailSender>();
builder.Services.AddScoped<BrixCMS.Open.Services.PageVersionService>();

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

// Populate HttpContext.User from the admin auth cookie, then evaluate any [Authorize] attributes.
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
    ctx.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
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

    // 5a. CMS catch-all — excludes blazor & api prefixes
    app.MapControllerRoute(
        name: "cms",
        pattern: "{*slug}",
        defaults: new { controller = "Cms", action = "Index", slug = "" },
        constraints: new { slug = @"^(?!_framework|api|_content|\.well-known).*$" });

    // 5b. Fallback to CMS for root URL / (catch-all {*slug} doesn't match empty path)
    app.MapFallbackToController("Index", "Cms");

    // 6. Blazor Web App (after CMS — only _framework / _blazor fall through)
    app.MapRazorComponents<BrixCMS.Open.Components.App>()
        .AddInteractiveServerRenderMode();

// =====================================================
// 🔟 INITIALIZATION — DB + PDF INGESTION
// =====================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BrixDbContext>();
    db.Database.EnsureCreated();

    // Manual migration: ApiKeys table (encrypted cloud AI keys)
    try
    {
        var sqlApiKeys = dbProvider switch
        {
            "sqlserver" => "IF OBJECT_ID(N'ApiKeys', N'U') IS NULL CREATE TABLE [ApiKeys] ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, [Provider] NVARCHAR(50) NOT NULL, [EncryptedKey] NVARCHAR(MAX) NOT NULL, [Iv] NVARCHAR(MAX) NOT NULL, [AuthTag] NVARCHAR(MAX) NOT NULL, [CreatedAt] NVARCHAR(50) NOT NULL, [UpdatedAt] NVARCHAR(50) NOT NULL)",
            "postgres"  => @"CREATE TABLE IF NOT EXISTS ""ApiKeys"" (""Id"" SERIAL PRIMARY KEY, ""Provider"" TEXT NOT NULL UNIQUE, ""EncryptedKey"" TEXT NOT NULL, ""Iv"" TEXT NOT NULL, ""AuthTag"" TEXT NOT NULL, ""CreatedAt"" TEXT NOT NULL, ""UpdatedAt"" TEXT NOT NULL)",
            _           => @"CREATE TABLE IF NOT EXISTS ""ApiKeys"" (""Id"" INTEGER PRIMARY KEY AUTOINCREMENT, ""Provider"" TEXT NOT NULL UNIQUE, ""EncryptedKey"" TEXT NOT NULL, ""Iv"" TEXT NOT NULL, ""AuthTag"" TEXT NOT NULL, ""CreatedAt"" TEXT NOT NULL, ""UpdatedAt"" TEXT NOT NULL)",
        };
        db.Database.ExecuteSqlRaw(sqlApiKeys);
    }
    catch { }

    // Manual migration: PageViews table
    try
    {
        var sqlPageViews = dbProvider switch
        {
            "sqlserver" => "IF OBJECT_ID(N'PageViews', N'U') IS NULL CREATE TABLE [PageViews] ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, [ViewedAt] NVARCHAR(50) NOT NULL, [Slug] NVARCHAR(500) NOT NULL, [UserAgent] NVARCHAR(MAX) NULL, [Referrer] NVARCHAR(MAX) NULL)",
            "postgres"  => @"CREATE TABLE IF NOT EXISTS ""PageViews"" (""Id"" SERIAL PRIMARY KEY, ""ViewedAt"" TEXT NOT NULL, ""Slug"" TEXT NOT NULL, ""UserAgent"" TEXT, ""Referrer"" TEXT)",
            _           => @"CREATE TABLE IF NOT EXISTS ""PageViews"" (""Id"" INTEGER PRIMARY KEY AUTOINCREMENT, ""ViewedAt"" TEXT NOT NULL, ""Slug"" TEXT NOT NULL, ""UserAgent"" TEXT, ""Referrer"" TEXT)",
        };
        db.Database.ExecuteSqlRaw(sqlPageViews);
    }
    catch { }

    // Manual migration: SEO columns on Pages table
    var newPageColumns = new Dictionary<string, string>
    {
        ["MetaDescription"] = "TEXT",
        ["OgImage"] = "TEXT",
        ["MetaKeywords"] = "TEXT",
        ["IsSeed"] = "INTEGER NOT NULL DEFAULT 0",
        ["ParentId"] = "TEXT",
    };
    foreach (var col in newPageColumns)
        AddColumnIfNotExists(db.Database, dbProvider, "Pages", col.Key, col.Value);
    // Mark the existing demo page as seed (applies to DBs created before this column existed)
    try { db.Database.ExecuteSqlRaw("UPDATE Pages SET IsSeed = 1 WHERE Title = 'Home' AND IsSeed = 0 AND (SELECT COUNT(*) FROM Pages WHERE IsSeed = 0 AND IsPublished = 1) <= 1"); }
    catch { }

    // ── Seed BrixCMS landing page (first run) ──
    BrixCMS.Open.Services.BrixLandingSeeder.SeedIfEmpty(db);

    // Manual migration: PageVersions table (full-page undo history)
    try
    {
        var sqlPageVersions = dbProvider switch
        {
            "sqlserver" => "IF OBJECT_ID(N'PageVersions', N'U') IS NULL CREATE TABLE [PageVersions] ([Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, [PageId] UNIQUEIDENTIFIER NOT NULL, [CreatedAt] DATETIME2 NOT NULL, [CreatedByEmail] NVARCHAR(500) NULL, [Label] NVARCHAR(100) NOT NULL, [Title] NVARCHAR(500) NOT NULL, [Slug] NVARCHAR(500) NULL, [JsonData] NVARCHAR(MAX) NULL, [MetaDescription] NVARCHAR(MAX) NULL, [OgImage] NVARCHAR(MAX) NULL, [MetaKeywords] NVARCHAR(MAX) NULL, [BlocksJson] NVARCHAR(MAX) NOT NULL)",
            "postgres"  => @"CREATE TABLE IF NOT EXISTS ""PageVersions"" (""Id"" UUID PRIMARY KEY, ""PageId"" UUID NOT NULL, ""CreatedAt"" TIMESTAMP NOT NULL, ""CreatedByEmail"" TEXT, ""Label"" TEXT NOT NULL, ""Title"" TEXT NOT NULL, ""Slug"" TEXT, ""JsonData"" TEXT, ""MetaDescription"" TEXT, ""OgImage"" TEXT, ""MetaKeywords"" TEXT, ""BlocksJson"" TEXT NOT NULL)",
            _           => @"CREATE TABLE IF NOT EXISTS ""PageVersions"" (""Id"" TEXT PRIMARY KEY, ""PageId"" TEXT NOT NULL, ""CreatedAt"" TEXT NOT NULL, ""CreatedByEmail"" TEXT, ""Label"" TEXT NOT NULL, ""Title"" TEXT NOT NULL, ""Slug"" TEXT, ""JsonData"" TEXT, ""MetaDescription"" TEXT, ""OgImage"" TEXT, ""MetaKeywords"" TEXT, ""BlocksJson"" TEXT NOT NULL)",
        };
        db.Database.ExecuteSqlRaw(sqlPageVersions);
        db.Database.ExecuteSqlRaw(dbProvider switch
        {
            "sqlserver" => "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PageVersions_PageId') CREATE INDEX [IX_PageVersions_PageId] ON [PageVersions] ([PageId])",
            "postgres"  => @"CREATE INDEX IF NOT EXISTS ""IX_PageVersions_PageId"" ON ""PageVersions"" (""PageId"")",
            _           => @"CREATE INDEX IF NOT EXISTS ""IX_PageVersions_PageId"" ON ""PageVersions"" (""PageId"")",
        });
    }
    catch { /* already exists */ }

    // Manual migration: Subscribers table
    try
    {
        var sqlSubscribers = dbProvider switch
        {
            "sqlserver" => "IF OBJECT_ID(N'Subscribers', N'U') IS NULL CREATE TABLE [Subscribers] ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, [Email] NVARCHAR(500) NOT NULL UNIQUE, [Name] NVARCHAR(MAX) NULL, [CreatedAt] NVARCHAR(50) NOT NULL DEFAULT (CONVERT(NVARCHAR(50), GETUTCDATE(), 126)))",
            "postgres"  => @"CREATE TABLE IF NOT EXISTS ""Subscribers"" (""Id"" SERIAL PRIMARY KEY, ""Email"" TEXT NOT NULL UNIQUE, ""Name"" TEXT, ""CreatedAt"" TEXT NOT NULL DEFAULT (NOW()::text))",
            _           => @"CREATE TABLE IF NOT EXISTS ""Subscribers"" (""Id"" INTEGER PRIMARY KEY AUTOINCREMENT, ""Email"" TEXT NOT NULL UNIQUE, ""Name"" TEXT, ""CreatedAt"" TEXT NOT NULL DEFAULT (datetime('now')))",
        };
        db.Database.ExecuteSqlRaw(sqlSubscribers);
    }
    catch { /* already exists */ }

    // Manual migration: AdminUser new columns (safe if already exist)
    var adminNewColumns = new Dictionary<string, string>
    {
        ["Name"]        = "TEXT NOT NULL DEFAULT ''",
        ["IsOwner"]     = "INTEGER NOT NULL DEFAULT 0",
        ["CreatedAt"]   = "TEXT NOT NULL DEFAULT '2024-01-01T00:00:00'",
        ["Role"]        = "TEXT",
        ["Permissions"] = "TEXT",
    };
    foreach (var col in adminNewColumns)
        AddColumnIfNotExists(db.Database, dbProvider, "AdminUsers", col.Key, col.Value);
    // Ensure the first admin is always owner
    try { db.Database.ExecuteSqlRaw("UPDATE AdminUsers SET IsOwner = 1 WHERE Id = (SELECT MIN(Id) FROM AdminUsers)"); }
    catch { }

    var startupLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    // ── Seed admin on first run ──
    if (!db.AdminUsers.Any())
    {
        const string defaultPassword = "admin123";
        var hash = BrixCMS.Open.Services.AdminAuthService.HashPassword(defaultPassword, out var salt);
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

        startupLogger.LogWarning("==========================================================");
        startupLogger.LogWarning("ADMIN CREATED — email: admin@brix.com  /  password: admin123");
        startupLogger.LogWarning("IMPORTANT: Change the password at /Manager/Admins after first login!");
        startupLogger.LogWarning("==========================================================");
    }

    // ── Password recovery: if AdminRecoveryPassword is set in appsettings, reset admin password ──
    var recoveryPassword = builder.Configuration["AdminRecoveryPassword"];
    if (!string.IsNullOrWhiteSpace(recoveryPassword))
    {
        var adminToReset = db.AdminUsers.OrderBy(u => u.Id).FirstOrDefault();
        if (adminToReset != null)
        {
            adminToReset.PasswordHash = BrixCMS.Open.Services.AdminAuthService.HashPassword(recoveryPassword, out var rSalt);
            adminToReset.PasswordSalt = rSalt;
            db.SaveChanges();
            startupLogger.LogWarning("==========================================================");
            startupLogger.LogWarning("ADMIN PASSWORD RESET via AdminRecoveryPassword");
            startupLogger.LogWarning("Email:    {Email}", adminToReset.Email);
            startupLogger.LogWarning("Password: {Password}", recoveryPassword);
            startupLogger.LogWarning("IMPORTANT: Remove AdminRecoveryPassword from appsettings.json after login!");
            startupLogger.LogWarning("==========================================================");
        }
    }

    // ── Always log admin email on startup so it's never forgotten ──
    var currentAdmin = db.AdminUsers.OrderBy(u => u.Id).FirstOrDefault();
    if (currentAdmin != null)
        startupLogger.LogInformation("Admin login email: {Email}", currentAdmin.Email);

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

        // Run ingestion in background so app doesn't block on startup
        // Timeout after 30s to avoid hanging if Ollama is unavailable
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        _ = Task.Run(async () =>
        {
            try
            {
                await DataIngestor.IngestDataAsync(
                    app.Services,
                    new PDFDirectorySource(dataPath, generator));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "⚠️ PDF ingestion failed (non-blocking, app continues)");
            }
        }, cts.Token);
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

// ─────────────────────────────────────────────────────────────────────────────
// LOCAL HELPERS
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Adds a column to a table only if it does not already exist, compatible with SQLite, SQL Server and Postgres.</summary>
static void AddColumnIfNotExists(
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade db,
    string provider, string table, string column, string sqliteTypeDef)
{
    try
    {
        switch (provider)
        {
            case "sqlserver":
                // Map SQLite type tokens to SQL Server equivalents
                var ssDef = sqliteTypeDef
                    .Replace("INTEGER NOT NULL DEFAULT 0", "INT NOT NULL DEFAULT 0")
                    .Replace("INTEGER", "INT")
                    .Replace("TEXT NOT NULL DEFAULT ''", "NVARCHAR(MAX) NOT NULL DEFAULT ''")
                    .Replace("TEXT NOT NULL DEFAULT '2024-01-01T00:00:00'", "NVARCHAR(50) NOT NULL DEFAULT '2024-01-01T00:00:00'")
                    .Replace("TEXT NOT NULL", "NVARCHAR(MAX) NOT NULL")
                    .Replace("TEXT", "NVARCHAR(MAX)");
                db.ExecuteSqlRaw(
                    $"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'{table}' AND COLUMN_NAME=N'{column}') " +
                    $"ALTER TABLE [{table}] ADD [{column}] {ssDef}");
                break;

            case "postgres":
                db.ExecuteSqlRaw(
                    $@"ALTER TABLE ""{table}"" ADD COLUMN IF NOT EXISTS ""{column}"" {sqliteTypeDef}");
                break;

            default: // sqlite — check first to avoid EF logging a "fail" entry
                var exists = db.SqlQueryRaw<int>(
                    $"SELECT COUNT(*) FROM pragma_table_info('{table}') WHERE name='{column}'")
                    .AsEnumerable().FirstOrDefault();
                if (exists == 0)
                    db.ExecuteSqlRaw(
                        $@"ALTER TABLE ""{table}"" ADD COLUMN ""{column}"" {sqliteTypeDef}");
                break;
        }
    }
    catch { /* column already exists — safe to ignore */ }
}

