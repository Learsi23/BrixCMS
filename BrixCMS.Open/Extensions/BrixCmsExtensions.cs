using BrixCMS.Open.Data;
using BrixCMS.Open.Extensions.Authorization;
using BrixCMS.Open.Services;
using BrixCMS.Open.Services.Email;
using BrixCMS.Open.Services.Ingestion;
using Markdig;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.InMemory;
using OllamaSharp;
using System.Threading.RateLimiting;

namespace BrixCMS.Open.Extensions;

/// <summary>
/// Extension methods for wiring BrixCMS into any ASP.NET Core 10 application.
///
/// Minimal headless setup (Content API + Admin only):
/// <code>
///   builder.Services.AddBrixCms(builder.Configuration);
///   var app = builder.Build();
///   await app.InitializeBrixCmsAsync();
///   app.UseStaticFiles();
///   app.UseRouting();
///   app.UseSession();
///   app.UseAuthentication();  // populates HttpContext.User from the admin auth cookie
///   app.UseAuthorization();   // evaluates [Authorize] attributes
///   app.UseAntiforgery();
///   app.MapBrixCmsAdmin();    // /Manager admin panel
///   app.MapBrixCmsApi();      // /api/content/* headless endpoints
///   app.Run();
/// </code>
/// </summary>
public static class BrixCmsExtensions
{
    // ── Service registration ──────────────────────────────────────────────────

    /// <summary>
    /// Registers all BrixCMS services (database, blocks, AI, email, session…).
    /// </summary>
    public static IServiceCollection AddBrixCms(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<BrixCmsOptions>? configure = null)
    {
        var opts = new BrixCmsOptions();
        configure?.Invoke(opts);
        opts.Apply(configuration);

        services.AddSingleton(opts);

        // ── Database ──────────────────────────────────────────────────────────
        var dbProvider = (configuration["DatabaseProvider"] ?? "sqlite").ToLowerInvariant();
        services.AddDbContext<BrixDbContext>(db =>
        {
            switch (dbProvider)
            {
                case "sqlserver":
                    db.UseSqlServer(configuration.GetConnectionString("SqlServer")
                        ?? throw new InvalidOperationException("ConnectionStrings:SqlServer required."));
                    break;
                case "postgres":
                    db.UseNpgsql(configuration.GetConnectionString("Postgres")
                        ?? throw new InvalidOperationException("ConnectionStrings:Postgres required."));
                    break;
                default:
                    db.UseSqlite(opts.SqliteConnectionString);
                    break;
            }
        });

        // ── Core CMS services ─────────────────────────────────────────────────
        services.AddScoped<ContentService>();
        services.AddScoped<AdminAuthService>();
        services.AddScoped<ApiKeyService>();
        services.AddSingleton<PromptsService>();
        services.AddTransient<EmailSender>();
        services.AddbrixBlocks();

        // ── HTTP infrastructure ───────────────────────────────────────────────
        services.AddHttpClient();
        services.AddResponseCaching();
        services.AddMemoryCache(o => o.SizeLimit = opts.ImageCacheSizeBytes);
        services.AddAntiforgery(o => o.HeaderName = "X-CSRF-TOKEN");
        services.AddDistributedMemoryCache();
        services.AddSession(o =>
        {
            o.Cookie.HttpOnly    = true;
            o.Cookie.IsEssential = true;
            o.Cookie.SameSite    = SameSiteMode.Strict;
            o.Cookie.Name        = opts.SessionCookieName;
            o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            o.IdleTimeout        = TimeSpan.FromMinutes(opts.SessionTimeoutMinutes);
        });

        // ── Authentication & authorization — admin identity ─────────────────────
        // Standard ASP.NET Core cookie auth: AdminAuthService.BuildPrincipal() maps an
        // AdminUser row to the ClaimsPrincipal signed in via HttpContext.SignInAsync at login.
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.Name = opts.SessionCookieName + ".Auth";
                o.Cookie.HttpOnly = true;
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.SameSite = SameSiteMode.Strict;
                o.ExpireTimeSpan = TimeSpan.FromHours(24);
                o.SlidingExpiration = true;
                o.LoginPath = "/admin/manager";
                o.AccessDeniedPath = "/Manager";
                o.Events.OnRedirectToLogin = ctx =>
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
        services.AddAdminAuthorization();

        // ── Rate limiting ─────────────────────────────────────────────────────
        services.AddRateLimiter(rl =>
        {
            rl.AddFixedWindowLimiter("login", o =>
            {
                o.PermitLimit = opts.LoginRateLimit;
                o.Window      = TimeSpan.FromMinutes(1);
                o.QueueLimit  = 0;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
            rl.AddFixedWindowLimiter("ai", o =>
            {
                o.PermitLimit = opts.AiRateLimit;
                o.Window      = TimeSpan.FromMinutes(1);
                o.QueueLimit  = 0;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
            rl.RejectionStatusCode = 429;
        });

        // ── Markdown ──────────────────────────────────────────────────────────
        services.AddSingleton<MarkdownPipeline>(_ =>
            new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseEmojiAndSmiley()
                .UseSoftlineBreakAsHardlineBreak()
                .Build());

        // ── AI / Ollama ───────────────────────────────────────────────────────
        var ollamaUrl      = configuration["Ollama:BaseUrl"]      ?? "http://localhost:11434";
        var embeddingModel = configuration["Ollama:EmbeddingModel"] ?? "all-minilm:latest";
        var chatModel      = configuration["Ollama:ChatModel"]      ?? "llama3.1:8b";

        services.AddEmbeddingGenerator(new OllamaApiClient(new Uri(ollamaUrl), embeddingModel));
        services.AddInMemoryVectorStore();
        services.AddInMemoryVectorStoreRecordCollection<string, IngestedChunk>("data-chatappollama-chunks");
        services.AddInMemoryVectorStoreRecordCollection<string, IngestedDocument>("data-chatappollama-documents");

        services.AddScoped<IChatClient>(sp =>
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

        services.AddScoped<DataIngestor>();
        services.AddSingleton<SemanticSearch>();

        return services;
    }

    // ── Middleware / routing ──────────────────────────────────────────────────

    /// <summary>Maps the BrixCMS admin panel routes (<c>/Manager/**</c>).</summary>
    public static IEndpointRouteBuilder MapBrixCmsAdmin(this IEndpointRouteBuilder app)
    {
        app.MapControllerRoute(
            name: "admin-login",
            pattern: "admin/manager",
            defaults: new { area = "Manager", controller = "Login", action = "Index" });

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Manager}/{action=Index}/{id?}");

        return app;
    }

    /// <summary>Maps the BrixCMS headless Content API (<c>/api/content/**</c>) and newsletter endpoint.</summary>
    public static IEndpointRouteBuilder MapBrixCmsApi(this IEndpointRouteBuilder app)
    {
        // API controller routes — handled automatically by MapControllers() if called in the host.
        // This method is a documentation / discoverability marker; actual mapping happens via
        // MapControllers() or MapControllerRoute() called in the host project.
        return app;
    }

    /// <summary>Maps the CMS page renderer catch-all (<c>/{slug?}</c>).</summary>
    public static IEndpointRouteBuilder MapBrixCmsPages(this IEndpointRouteBuilder app)
    {
        app.MapControllerRoute(
            name: "cms",
            pattern: "{slug?}",
            defaults: new { controller = "Cms", action = "Index" },
            constraints: new { slug = @"^(?!_framework|api|_content|\.well-known).*$" });

        return app;
    }

    // ── Database initialisation ───────────────────────────────────────────────

    /// <summary>
    /// Creates the database, runs migrations, seeds demo content and ingest PDFs.
    /// Call this once after <c>builder.Build()</c> and before <c>app.Run()</c>.
    /// </summary>
    public static async Task InitializeBrixCmsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db     = scope.ServiceProvider.GetRequiredService<BrixDbContext>();
        var opts   = scope.ServiceProvider.GetRequiredService<BrixCmsOptions>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BrixCmsOptions>>();

        db.Database.EnsureCreated();

        // ── Seed admin on first run ──
        if (!db.AdminUsers.Any())
        {
            const string defaultPassword = "admin123";
            var hash = AdminAuthService.HashPassword(defaultPassword, out var salt);
            db.AdminUsers.Add(new AdminUser
            {
                Email        = opts.DefaultAdminEmail,
                Name         = "Owner",
                PasswordHash = hash,
                PasswordSalt = salt,
                IsOwner      = true,
                CreatedAt    = DateTime.UtcNow
            });
            db.SaveChanges();

            logger.LogWarning("Admin created — email: {Email}  password: admin123", opts.DefaultAdminEmail);
            logger.LogWarning("Change the password at /Manager/Admins after first login!");
        }

        // ── Seed demo pages on first run ──
        BrixLandingSeeder.SeedIfEmpty(db);

        // ── PDF ingestion ──
        var webRoot  = app.Environment.WebRootPath ?? "wwwroot";
        var dataPath = Path.Combine(webRoot, "Data");

        if (Directory.Exists(dataPath))
        {
            var generator = scope.ServiceProvider
                .GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

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
    }
}

/// <summary>BrixCMS configuration options.</summary>
public sealed class BrixCmsOptions
{
    public string SqliteConnectionString { get; set; } = "Data Source=brix.db";
    public string SessionCookieName      { get; set; } = ".BrixCMS.Session";
    public int    SessionTimeoutMinutes  { get; set; } = 60;
    public long   ImageCacheSizeBytes    { get; set; } = 256 * 1024 * 1024; // 256 MB
    public int    LoginRateLimit         { get; set; } = 5;
    public int    AiRateLimit            { get; set; } = 20;
    public string DefaultAdminEmail      { get; set; } = "admin@brix.com";

    internal void Apply(IConfiguration cfg)
    {
        if (!string.IsNullOrWhiteSpace(cfg["Brix:SessionCookieName"]))
            SessionCookieName = cfg["Brix:SessionCookieName"]!;
        if (int.TryParse(cfg["Brix:SessionTimeoutMinutes"], out var t))
            SessionTimeoutMinutes = t;
        if (!string.IsNullOrWhiteSpace(cfg["Brix:DefaultAdminEmail"]))
            DefaultAdminEmail = cfg["Brix:DefaultAdminEmail"]!;
    }
}
