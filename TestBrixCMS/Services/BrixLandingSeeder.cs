using TestBrixCMS.Data;
using System.Text.Json;

namespace TestBrixCMS.Services;

/// <summary>
/// Seeds the TestBrixCMS home page and site config on first run.
/// Mirrors the dark-tech style of the brix-cms Next.js seed.
/// </summary>
public static class BrixLandingSeeder
{
    public static void SeedIfEmpty(BrixDbContext db)
    {
        SeedHomePage(db);
        SeedSiteConfig(db);
    }

    // ── Home page ─────────────────────────────────────────────────────────────

    private static void SeedHomePage(BrixDbContext db)
    {
        // Only seed on a truly empty database (no pages AND no site config)
        if (db.Pages.Any() || db.SiteConfig.Any(c => c.Key == "site"))
            return;

        var page = new Page
        {
            Id          = Guid.NewGuid(),
            Title       = "Home",
            Slug        = "",
            IsPublished = true,
            IsSeed      = true,
            PublishedAt = DateTime.UtcNow,
            PageType    = "standard",
            JsonData    = JsonSerializer.Serialize(new { BackgroundColor = new { Value = "#ffffff" } }),
        };
        db.Pages.Add(page);

        int sort = 0;
        var blocks = new List<Block>();

        Block B(string type, object fields, Guid? parentId = null)
        {
            var b = new Block
            {
                Id        = Guid.NewGuid(),
                PageId    = page.Id,
                Type      = type,
                SortOrder = sort++,
                ParentId  = parentId,
                JsonData  = JsonSerializer.Serialize(fields),
            };
            blocks.Add(b);
            return b;
        }

        static object V(string value) => new { Value = value };

        // ── Block 1: Hero ─────────────────────────────────────────────────────
        B("HeroBlock", new
        {
            Title           = V("Build websites visually. Zero code needed."),
            TitleColor      = V("#ffffff"),
            TitleSize       = V("3.75rem"),
            Subtitle        = V("Open-source .NET block-based CMS — 40+ blocks, PDF chatbot, one-command setup."),
            SubtitleColor   = V("#94a3b8"),
            SubtitleSize    = V("1.2rem"),
            Description     = V(""),
            Background      = V("/images/HeroBrix.png"),
            BackgroundColor = V("#0f172a"),
            OverlayColor    = V("#0f172a"),
            OverlayOpacity  = V("0.78"),
            Height          = V("half-screen"),
            TextAlign       = V("center"),
            ButtonText      = V("Open Admin →"),
            ButtonUrl       = V("/Manager"),
            ButtonColor     = V("#10b981"),
            ButtonTextColor = V("#ffffff"),
        });

        // ── Block 2: Stats ────────────────────────────────────────────────────
        B("StatsBlock", new
        {
            Title           = V(""),
            Subtitle        = V(""),
            Stat1Number     = V("40+"),
            Stat1Label      = V("Pre-built blocks"),
            Stat1Icon       = V("fas fa-th-large"),
            Stat2Number     = V("MIT"),
            Stat2Label      = V("Open source license"),
            Stat2Icon       = V("fas fa-code-branch"),
            Stat3Number     = V(".NET 10"),
            Stat3Label      = V("Blazor Server + MVC"),
            Stat3Icon       = V("fas fa-bolt"),
            Stat4Number     = V("PDF"),
            Stat4Label      = V("Chatbot included"),
            Stat4Icon       = V("fas fa-robot"),
            NumberColor     = V("#10b981"),
            LabelColor      = V("#94a3b8"),
            BackgroundColor = V("#0f172a"),
            CardBgColor     = V("#1e293b"),
            PaddingY        = V("4rem"),
        });

        // ── Block 3: Section heading ──────────────────────────────────────────
        B("TextBlock", new
        {
            Title             = V("Everything you need to build fast"),
            TitleColor        = V("#0f172a"),
            TitleSize         = V("2.25rem"),
            TitleWeight       = V("800"),
            TitleAlignment    = V("center"),
            Subtitle          = V("No vendor lock-in. Your data, your server, your rules."),
            SubtitleColor     = V("#64748b"),
            SubtitleSize      = V("1.1rem"),
            SubtitleAlignment = V("center"),
            Body              = V(""),
            PaddingTop        = V("4rem"),
            PaddingBottom     = V("0.5rem"),
            PaddingLeft       = V("1.5rem"),
            PaddingRight      = V("1.5rem"),
        });

        // ── Block 4: Features grid ────────────────────────────────────────────
        var featGrid = B("GridColumn", new
        {
            MaxColumns      = V("3"),
            Gap             = V("gap-6"),
            PaddingY        = V("3rem"),
            PaddingX        = V("1.5rem"),
            BackgroundColor = V("#ffffff"),
        });

        B("IconCardBlock", new
        {
            LeftIconClass   = V("fas fa-th-large"),
            LeftIconColor   = V("#10b981"),
            LeftIconFaSize  = V("2rem"),
            IconPosition    = V("top"),
            TextAlign       = V("left"),
            Title           = V("40+ pre-built blocks"),
            TitleColor      = V("#0f172a"),
            TitleSize       = V("1.15rem"),
            Text            = V("Hero, Stats, Testimonials, Team, Gallery, Map, Countdown, Timeline, Accordion, Tabs, PDF Chatbot — and more. Add your own in minutes."),
            TextColor       = V("#64748b"),
            TextSize        = V("0.95rem"),
            BackgroundColor = V("#f8fafc"),
            BorderColor     = V("#e2e8f0"),
            BorderWidth     = V("1px"),
            BorderRadius    = V("16px"),
            Padding         = V("1.75rem"),
            Shadow          = V("0 1px 4px rgba(0,0,0,0.06)"),
        }, featGrid.Id);

        B("IconCardBlock", new
        {
            LeftIconClass   = V("fas fa-robot"),
            LeftIconColor   = V("#6366f1"),
            LeftIconFaSize  = V("2rem"),
            IconPosition    = V("top"),
            TextAlign       = V("left"),
            Title           = V("PDF Chatbot built-in"),
            TitleColor      = V("#0f172a"),
            TitleSize       = V("1.15rem"),
            Text            = V("Drop PDFs in wwwroot/Data and get an AI chatbot that answers from your documents. Works with Ollama (local) and all its supported models — private, powerful, and ready to go."),
            TextColor       = V("#64748b"),
            TextSize        = V("0.95rem"),
            BackgroundColor = V("#f8fafc"),
            BorderColor     = V("#e2e8f0"),
            BorderWidth     = V("1px"),
            BorderRadius    = V("16px"),
            Padding         = V("1.75rem"),
            Shadow          = V("0 1px 4px rgba(0,0,0,0.06)"),
        }, featGrid.Id);

        B("IconCardBlock", new
        {
            LeftIconClass   = V("fas fa-rocket"),
            LeftIconColor   = V("#f59e0b"),
            LeftIconFaSize  = V("2rem"),
            IconPosition    = V("top"),
            TextAlign       = V("left"),
            Title           = V("Deploy anywhere"),
            TitleColor      = V("#0f172a"),
            TitleSize       = V("1.15rem"),
            Text            = V("SQLite by default — zero database config. Self-host on any VPS, Railway, Render, or Azure in minutes."),
            TextColor       = V("#64748b"),
            TextSize        = V("0.95rem"),
            BackgroundColor = V("#f8fafc"),
            BorderColor     = V("#e2e8f0"),
            BorderWidth     = V("1px"),
            BorderRadius    = V("16px"),
            Padding         = V("1.75rem"),
            Shadow          = V("0 1px 4px rgba(0,0,0,0.06)"),
        }, featGrid.Id);

        // ── Block 5: Spacer ───────────────────────────────────────────────────
        B("SpacerBlock", new { Height = V("3rem"), BackgroundColor = V("#ffffff") });

        // ── Block 6: CTA Banner ───────────────────────────────────────────────
        B("CTABannerBlock", new
        {
            Title            = V("Ready to build?"),
            TitleColor       = V("#ffffff"),
            TitleSize        = V("2.5rem"),
            Subtitle         = V("Clone the repo, run dotnet run, and you're live in under 2 minutes."),
            SubtitleColor    = V("rgba(255,255,255,0.7)"),
            Btn1Text         = V("View on GitHub"),
            Btn1Url          = V("https://github.com/Learsi23/BrixCMS"),
            Btn1BgColor      = V("#10b981"),
            Btn1TextColor    = V("#ffffff"),
            Btn2Text         = V("Open Admin Panel"),
            Btn2Url          = V("/Manager/login"),
            Btn2Color        = V("rgba(255,255,255,0.15)"),
            BackgroundColor  = V("#0f172a"),
            BackgroundColor2 = V("#1e293b"),
            PaddingY         = V("6rem"),
            TextAlign        = V("center"),
        });

        db.Blocks.AddRange(blocks);
        db.SaveChanges();
    }

    // ── Site config (navbar + footer) ─────────────────────────────────────────

    private static void SeedSiteConfig(BrixDbContext db)
    {
        if (db.SiteConfig.Any(c => c.Key == "site"))
            return;

        var config = new
        {
            navbar = new
            {
                backgroundColor = "#ffffff",
                textColor       = "#0f172a",
                logo            = "/images/logo-menu.png",
                logoAltText     = "BrixCMS",
                logoWidth       = "120px",
                logoLink        = "/",
                isSticky        = true,
                hasShadow       = true,
                paddingVertical = "py-3",
                menuItems       = new[]
                {
                    new { customText = "GitHub", customUrl = "https://github.com/Learsi23/BrixCMS", isCustomUrl = true, pageSlug = "" },
                    new { customText = "Admin",  customUrl = "/Manager/login", isCustomUrl = true, pageSlug = "" },
                },
            },
            footer = new
            {
                backgroundColor       = "#0f172a",
                textColor             = "#94a3b8",
                logo                  = "/images/logo-menu.png",
                logoAltText           = "BrixCMS",
                logoWidth             = "100px",
                logoPosition          = "left",
                showPagesColumn       = false,
                pagesColumnTitle      = "Pages",
                pages                 = Array.Empty<object>(),
                showSocialMediaColumn = true,
                socialMediaColumnTitle = "Links",
                socialMedia           = new[]
                {
                    new { platform = "github", url = "https://github.com/Learsi23/BrixCMS", iconClass = "fab fa-github" },
                },
                showCopyrightRow    = true,
                companyName         = "BrixCMS",
                companyNumber       = "",
                copyrightText       = "MIT License — free forever",
                showHorizontalLine  = true,
                paddingVertical     = "py-8",
                columnsGap          = "gap-8",
            },
        };

        db.SiteConfig.Add(new SiteConfig
        {
            Key      = "site",
            JsonData = JsonSerializer.Serialize(config),
        });
        db.SaveChanges();
    }
}
