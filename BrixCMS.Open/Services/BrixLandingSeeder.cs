using BrixCMS.Open.Data;
using System.Text.Json;

namespace BrixCMS.Open.Services;

/// <summary>
/// Seeds 3 showcase pages on first run:
///   /          Home      — BrixCMS.Open overview + "meet Pro" teaser
///   /features  Features  — Full block library + Ollama AI + quick-start
///   /pro       Pro       — BrixCMS Pro upsell with comparison table + FAQ
/// </summary>
public static class BrixLandingSeeder
{
    // ─────────────────────────────────────────────────────────────────────────
    // Pro brand palette (matches index.html / byraer.html)
    // ─────────────────────────────────────────────────────────────────────────
    const string BG       = "#0A0A0B";
    const string SURFACE  = "#111113";
    const string SURFACE2 = "#18181C";
    const string ACCENT   = "#5B6EF5";
    const string SUCCESS  = "#22C55E";
    const string WARNING  = "#F59E0B";
    const string TEXT     = "#F0F0F5";
    const string TEXT2    = "#9696A6";
    const string BORDER   = "#2A2A30";

    // ─────────────────────────────────────────────────────────────────────────

    public static void SeedIfEmpty(BrixDbContext db)
    {
        if (db.Pages.Any() || db.SiteConfig.Any(c => c.Key == "site"))
            return;

        SeedHomePage(db);
        SeedFeaturesPage(db);
        SeedProPage(db);
        SeedSiteConfig(db);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    static object V(string? value) => new { Value = value ?? "" };

    static Block MakeBlock(Guid pageId, string type, object fields, int order, Guid? parentId = null) => new()
    {
        Id        = Guid.NewGuid(),
        PageId    = pageId,
        ParentId  = parentId,
        Type      = type,
        SortOrder = order,
        JsonData  = JsonSerializer.Serialize(fields),
    };

    // ═══════════════════════════════════════════════════════════════════════
    //  PAGE 1 — HOME  (slug: "")
    // ═══════════════════════════════════════════════════════════════════════

    private static void SeedHomePage(BrixDbContext db)
    {
        var id   = Guid.NewGuid();
        var page = new Page
        {
            Id              = id,
            Title           = "Home",
            Slug            = "",
            IsPublished     = true,
            IsSeed          = true,
            PublishedAt     = DateTime.UtcNow,
            SortOrder       = 0,
            PageType        = "standard",
            MetaDescription = "BrixCMS.Open — free, self-hosted, open-source .NET 10 CMS. 40+ blocks, Ollama AI chatbot, SQLite, MIT license.",
            JsonData        = JsonSerializer.Serialize(new { BackgroundColor = new { Value = BG } }),
        };
        db.Pages.Add(page);

        int sort = 0;
        var blocks = new List<Block>();
        Block B(string type, object fields, Guid? parentId = null)
        {
            var b = MakeBlock(id, type, fields, sort++, parentId);
            blocks.Add(b);
            return b;
        }

        // ── Top announcement bar ──────────────────────────────────────────────
        B("BannerBlock", new
        {
            Icon            = V("✦"),
            Text            = V("These pages are built 100% with BrixCMS — visual editor, zero custom code."),
            LinkText        = V("Watch on YouTube →"),
            LinkUrl         = V("https://www.youtube.com/@BrixCMS"),
            BackgroundColor = V(ACCENT),
            TextColor       = V("#ffffff"),
            Closeable       = V("true"),
        });

        // ── Hero ──────────────────────────────────────────────────────────────
        B("HeroBlock", new
        {
            Title              = V("Your CMS. Your server. AI included."),
            TitleColor         = V(TEXT),
            TitleSize          = V("3.5rem"),
            Subtitle           = V("BrixCMS.Open is a free, open-source .NET 10 block-based CMS with 40+ visual blocks, Ollama AI chatbot, and zero configuration."),
            SubtitleColor      = V(TEXT2),
            SubtitleSize       = V("1.15rem"),
            Description        = V(""),
            BackgroundColor    = V(BG),
            BackgroundGradient = V("radial-gradient(ellipse 80% 55% at 50% 0%, rgba(91,110,245,0.12) 0%, transparent 70%)"),
            OverlayColor       = V(BG),
            OverlayOpacity     = V("0.0"),
            Height             = V("half-screen"),
            TextAlign          = V("center"),
            ShowGridPattern    = V("true"),
            GridPatternColor   = V(BORDER),
            ButtonText         = V("Open Admin →"),
            ButtonUrl          = V("/Manager"),
            ButtonColor        = V(ACCENT),
            ButtonTextColor    = V("#ffffff"),
        });

        // ── Stats ─────────────────────────────────────────────────────────────
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
            Stat4Number     = V("Ollama"),
            Stat4Label      = V("Local AI — free, private"),
            Stat4Icon       = V("fas fa-robot"),
            NumberColor     = V(ACCENT),
            LabelColor      = V(TEXT2),
            BackgroundColor = V(SURFACE),
            CardBgColor     = V(SURFACE2),
            PaddingY        = V("3rem"),
        });

        // ── Section: Features ─────────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("Everything you need to build fast"),
            TitleColor     = V(TEXT),
            TitleSize      = V("2.2rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("center"),
            Subtitle       = V("Self-hosted. No vendor lock-in. Your data, your server, your rules."),
            SubtitleColor  = V(TEXT2),
            SubtitleSize   = V("1rem"),
            SubtitleAlignment = V("center"),
            BackgroundColor = V(BG),
            Padding        = V("4rem 1.5rem 1.5rem"),
        });

        var featGrid = B("GridColumn", new
        {
            MaxColumns      = V("3"),
            Gap             = V("gap-5"),
            PaddingY        = V("1.5rem"),
            PaddingX        = V("1.5rem"),
            BackgroundColor = V(BG),
        });

        // 6 feature cards
        foreach (var (icon, color, title, text) in new[]
        {
            ("fas fa-th-large", ACCENT,
             "40+ pre-built blocks",
             "Hero, Stats, FAQ, Testimonials, Gallery, Map, Countdown, Timeline, Tabs, Team, Table, QR, Code and more. Add your own in minutes."),
            ("fas fa-robot", SUCCESS,
             "Ollama AI chatbot — local & private",
             "Drop PDFs in /wwwroot/Data and get an instant knowledge-base chatbot. Runs 100% locally via Ollama — no API key, no cost, no data leaving your server."),
            ("fas fa-database", WARNING,
             "SQLite — zero config",
             "Ships with SQLite out of the box. No database server to install or configure. Schema migrations run automatically at startup."),
            ("fas fa-rocket", ACCENT,
             "Deploy anywhere in 2 minutes",
             "Self-host on any VPS, Railway, Render or Azure. Single command: dotnet run. SQLite database created automatically."),
            ("fas fa-shield-alt", SUCCESS,
             "Secure by default",
             "Session auth, CSRF protection via [AutoValidateAntiforgeryToken], 2FA support (TOTP), rate-limited login and BCrypt password hashing."),
            ("fab fa-github", TEXT,
             "MIT License — free forever",
             "100% open source on GitHub. Fork it, modify it, host it, sell services on top of it. No licensing fees, ever."),
        })
        {
            B("IconCardBlock", new
            {
                LeftIconClass   = V(icon),
                LeftIconColor   = V(color),
                LeftIconFaSize  = V("1.75rem"),
                IconPosition    = V("top"),
                TextAlign       = V("left"),
                Title           = V(title),
                TitleColor      = V(TEXT),
                TitleSize       = V("1rem"),
                Text            = V(text),
                TextColor       = V(TEXT2),
                TextSize        = V("0.875rem"),
                BackgroundColor = V(SURFACE),
                BorderColor     = V(BORDER),
                BorderWidth     = V("1px"),
                BorderRadius    = V("12px"),
                Padding         = V("1.5rem"),
            }, featGrid.Id);
        }

        // ── Section: How it works ─────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("Up and running in under 2 minutes"),
            TitleColor     = V(TEXT),
            TitleSize      = V("2rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("center"),
            Subtitle       = V("No database config, no environment setup, no paid subscriptions."),
            SubtitleColor  = V(TEXT2),
            SubtitleAlignment = V("center"),
            BackgroundColor = V(SURFACE),
            Padding        = V("4rem 1.5rem 1rem"),
        });

        var timeline = B("TimelineBlock", new
        {
            Title           = V(""),
            TitleColor      = V(TEXT),
            Subtitle        = V(""),
            BackgroundColor = V(SURFACE),
            ConnectorColor  = V(ACCENT),
            Layout          = V("vertical"),
            TextAlign       = V("left"),
        });

        B("TimelineItemBlock", new
        {
            StepLabel   = V("01"),
            Icon        = V("fas fa-terminal"),
            Title       = V("Install & run"),
            Description = V("dotnet new install BrixCMS.Open.Templates && dotnet new brixcms -n MyWebsite && dotnet run — SQLite creates itself, seed pages load automatically."),
            AccentColor = V(ACCENT),
        }, timeline.Id);

        B("TimelineItemBlock", new
        {
            StepLabel   = V("02"),
            Icon        = V("fas fa-robot"),
            Title       = V("Connect Ollama (optional)"),
            Description = V("Install Ollama, pull any model (e.g. ollama pull llama3.2), paste the URL in Admin → AI Config. Get a fully private AI chatbot for zero cost."),
            AccentColor = V(SUCCESS),
        }, timeline.Id);

        B("TimelineItemBlock", new
        {
            StepLabel   = V("03"),
            Icon        = V("fas fa-paint-brush"),
            Title       = V("Build visually"),
            Description = V("Open the page editor, drag & drop 40+ blocks, configure colors, text and images. Publish with one click. Your site is live."),
            AccentColor = V(WARNING),
        }, timeline.Id);

        // ── Section: Pro teaser ───────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("Need the full platform?"),
            TitleColor     = V(TEXT),
            TitleSize      = V("2.2rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("center"),
            Subtitle       = V("BrixCMS.Open is free forever. BrixCMS Pro adds multi-tenant management, white label branding, BYOK AI (Gemini · DeepSeek · Mistral), Stripe e-commerce and Figma import."),
            SubtitleColor  = V(TEXT2),
            SubtitleAlignment = V("center"),
            BackgroundColor = V(BG),
            Padding        = V("4rem 1.5rem 1.5rem"),
        });

        var proGrid = B("GridColumn", new
        {
            MaxColumns      = V("3"),
            Gap             = V("gap-4"),
            PaddingY        = V("1rem"),
            PaddingX        = V("1.5rem"),
            BackgroundColor = V(BG),
        });

        foreach (var (icon, title, text) in new[]
        {
            ("fas fa-sitemap",       "Multi-tenant panel",                   "One dashboard for unlimited client sites. Each fully isolated."),
            ("fas fa-tag",           "White label",                          "Your logo, your domain, your brand. Zero BrixCMS branding visible."),
            ("fas fa-key",           "BYOK — Gemini · DeepSeek · Mistral",   "Connect your own API keys. Pay AI providers at their published rates. No markup ever."),
            ("fab fa-figma",         "Figma → CMS import",                   "AI reads your Figma file and builds the page blocks automatically."),
            ("fas fa-shopping-cart", "Stripe e-commerce (1% fee)",           "Sell products from your site. Lowest transaction fee in the market."),
            ("fas fa-headset",       "Priority support + SLA",               "Direct access to the BrixCMS team. On-boarding session included."),
        })
        {
            B("IconCardBlock", new
            {
                LeftIconClass   = V(icon),
                LeftIconColor   = V(ACCENT),
                LeftIconFaSize  = V("1.5rem"),
                IconPosition    = V("top"),
                TextAlign       = V("left"),
                Title           = V(title),
                TitleColor      = V(TEXT),
                TitleSize       = V("0.95rem"),
                Text            = V(text),
                TextColor       = V(TEXT2),
                TextSize        = V("0.85rem"),
                BackgroundColor = V(SURFACE),
                BorderColor     = V("#5B6EF540"),
                BorderWidth     = V("1px"),
                BorderRadius    = V("12px"),
                Padding         = V("1.25rem"),
            }, proGrid.Id);
        }

        // ── CTA ───────────────────────────────────────────────────────────────
        B("CTABannerBlock", new
        {
            Title            = V("Start building — it's free"),
            TitleColor       = V(TEXT),
            TitleSize        = V("2.25rem"),
            Subtitle         = V("Open source. Self-hosted. MIT license. No subscriptions."),
            SubtitleColor    = V(TEXT2),
            Btn1Text         = V("See all features →"),
            Btn1Url          = V("/features"),
            Btn1BgColor      = V(ACCENT),
            Btn1TextColor    = V("#ffffff"),
            Btn2Text         = V("BrixCMS Pro →"),
            Btn2Url          = V("/pro"),
            Btn2Color        = V(SUCCESS),
            BackgroundColor  = V(SURFACE),
            BackgroundColor2 = V(SURFACE2),
            PaddingY         = V("5rem"),
            TextAlign        = V("center"),
        });

        db.Blocks.AddRange(blocks);
        db.SaveChanges();
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  PAGE 2 — FEATURES  (slug: "features")
    // ═══════════════════════════════════════════════════════════════════════

    private static void SeedFeaturesPage(BrixDbContext db)
    {
        var id   = Guid.NewGuid();
        var page = new Page
        {
            Id              = id,
            Title           = "Features",
            Slug            = "features",
            IsPublished     = true,
            IsSeed          = true,
            PublishedAt     = DateTime.UtcNow,
            SortOrder       = 1,
            PageType        = "standard",
            MetaDescription = "BrixCMS.Open features — 40+ blocks, Ollama AI chatbot, visual page editor, SQLite, MIT license.",
            JsonData        = JsonSerializer.Serialize(new { BackgroundColor = new { Value = BG } }),
        };
        db.Pages.Add(page);

        int sort = 0;
        var blocks = new List<Block>();
        Block B(string type, object fields, Guid? parentId = null)
        {
            var b = MakeBlock(id, type, fields, sort++, parentId);
            blocks.Add(b);
            return b;
        }

        // ── Hero ──────────────────────────────────────────────────────────────
        B("HeroBlock", new
        {
            Title              = V("40+ blocks. Zero config."),
            TitleColor         = V(TEXT),
            TitleSize          = V("3.25rem"),
            Subtitle           = V("Every block in BrixCMS.Open — visual editor, live preview, and Ollama AI chatbot built in."),
            SubtitleColor      = V(TEXT2),
            BackgroundColor    = V(BG),
            BackgroundGradient = V("radial-gradient(ellipse 70% 50% at 60% 0%, rgba(91,110,245,0.1) 0%, transparent 70%)"),
            OverlayOpacity     = V("0.0"),
            Height             = V("quarter-screen"),
            TextAlign          = V("center"),
            ShowGridPattern    = V("true"),
            GridPatternColor   = V(BORDER),
            ButtonText         = V("Open Admin →"),
            ButtonUrl          = V("/Manager"),
            ButtonColor        = V(ACCENT),
            ButtonTextColor    = V("#ffffff"),
        });

        // ── AI Section ───────────────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("AI — local, private, completely free"),
            TitleColor     = V(TEXT),
            TitleSize      = V("1.9rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("left"),
            BackgroundColor = V(BG),
            Padding        = V("4rem 1.5rem 0.5rem"),
        });

        var aiGrid = B("GridColumn", new
        {
            MaxColumns      = V("2"),
            Gap             = V("gap-5"),
            PaddingY        = V("1rem"),
            PaddingX        = V("1.5rem"),
            BackgroundColor = V(BG),
        });

        B("IconCardBlock", new
        {
            LeftIconClass   = V("fas fa-robot"),
            LeftIconColor   = V(SUCCESS),
            LeftIconFaSize  = V("1.75rem"),
            IconPosition    = V("left"),
            TextAlign       = V("left"),
            Title           = V("Ollama chatbot — PDF knowledge base"),
            TitleColor      = V(TEXT),
            TitleSize       = V("1.05rem"),
            Text            = V("Drop any PDF files into /wwwroot/Data. The chatbot reads them and answers questions — grounded in your documents. Works with Llama 3, Mistral, Phi-3, Gemma or any Ollama-compatible model. No API key, no cost, no data leaving your server."),
            TextColor       = V(TEXT2),
            BackgroundColor = V(SURFACE),
            BorderColor     = V(BORDER),
            BorderWidth     = V("1px"),
            BorderRadius    = V("12px"),
            Padding         = V("1.5rem"),
        }, aiGrid.Id);

        B("IconCardBlock", new
        {
            LeftIconClass   = V("fas fa-comments"),
            LeftIconColor   = V(ACCENT),
            LeftIconFaSize  = V("1.75rem"),
            IconPosition    = V("left"),
            TextAlign       = V("left"),
            Title           = V("Inline chat block + floating widget"),
            TitleColor      = V(TEXT),
            TitleSize       = V("1.05rem"),
            Text            = V("Add the ChatBlock anywhere on a page for inline chat, or enable the FloatingChatBlock for a persistent chat bubble across all pages. Both powered by your local Ollama model."),
            TextColor       = V(TEXT2),
            BackgroundColor = V(SURFACE),
            BorderColor     = V(BORDER),
            BorderWidth     = V("1px"),
            BorderRadius    = V("12px"),
            Padding         = V("1.5rem"),
        }, aiGrid.Id);

        // ── Block Library ─────────────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("Block library"),
            TitleColor     = V(TEXT),
            TitleSize      = V("1.9rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("left"),
            BackgroundColor = V(BG),
            Padding        = V("3.5rem 1.5rem 0.5rem"),
        });

        var blockGrid = B("GridColumn", new
        {
            MaxColumns      = V("4"),
            Gap             = V("gap-3"),
            PaddingY        = V("1rem"),
            PaddingX        = V("1.5rem"),
            BackgroundColor = V(BG),
        });

        foreach (var (icon, name, desc) in new[]
        {
            ("fas fa-image",             "HeroBlock",           "Full-screen hero with image, overlay, CTA buttons"),
            ("fas fa-chart-bar",         "StatsBlock",          "4 key metrics with icons and labels"),
            ("fas fa-font",              "TextBlock",           "Rich text: title, subtitle, body + padding control"),
            ("fas fa-th",               "GridColumn",          "Responsive layout container (2–4 cols)"),
            ("fas fa-id-card",          "IconCardBlock",       "Card with FontAwesome icon, title and body text"),
            ("fas fa-question-circle",  "FAQBlock",            "Accordion FAQ (cards, bordered or list style)"),
            ("fas fa-star",             "TestimonialsBlock",   "Review carousel with avatars and star ratings"),
            ("fas fa-stream",           "TimelineBlock",       "Step-by-step process or company history"),
            ("fas fa-table",            "TableBlock",          "Data table — pipe-separated, striped, bordered or minimal"),
            ("fas fa-code",             "CodeBlock",           "Syntax-highlighted code with copy button"),
            ("fas fa-qrcode",           "QRCodeBlock",         "QR generator — URL, WiFi, vCard, Email"),
            ("fas fa-images",           "GalleryBlock",        "Image gallery with lightbox"),
            ("fas fa-play-circle",      "VideoBlock",          "YouTube / Vimeo / self-hosted video embed"),
            ("fas fa-map-marker-alt",   "MapBlock",            "Embedded Google or OpenStreetMap"),
            ("fas fa-clock",            "CountdownBlock",      "Live countdown timer to a target date"),
            ("fas fa-users",            "TeamBlock",           "Team members grid with photo, name & role"),
            ("fas fa-envelope",         "NewsletterBlock",     "Email signup form with configurable endpoint"),
            ("fas fa-phone",            "ContactFormBlock",    "Full contact form with validation"),
            ("fas fa-music",            "AudioBlock",          "Audio player (minimal, card or full style)"),
            ("fas fa-magic",            "LottieBlock",         "Lottie JSON animations (auto-play, loop, speed)"),
            ("fas fa-exchange-alt",     "BeforeAfterBlock",    "Before / after image comparison slider"),
            ("fas fa-layer-group",      "TabsBlock",           "Tabbed content panels"),
            ("fas fa-align-left",       "MarkdownBlock",       "Markdown-rendered content"),
        })
        {
            B("IconCardBlock", new
            {
                LeftIconClass   = V(icon),
                LeftIconColor   = V(ACCENT),
                LeftIconFaSize  = V("1.1rem"),
                IconPosition    = V("top"),
                TextAlign       = V("left"),
                Title           = V(name),
                TitleColor      = V(TEXT),
                TitleSize       = V("0.85rem"),
                Text            = V(desc),
                TextColor       = V(TEXT2),
                TextSize        = V("0.78rem"),
                BackgroundColor = V(SURFACE),
                BorderColor     = V(BORDER),
                BorderWidth     = V("1px"),
                BorderRadius    = V("10px"),
                Padding         = V("1.1rem"),
            }, blockGrid.Id);
        }

        // ── Quick Start ───────────────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("Get started in seconds"),
            TitleColor     = V(TEXT),
            TitleSize      = V("1.9rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("center"),
            BackgroundColor = V(SURFACE),
            Padding        = V("4rem 1.5rem 1rem"),
        });

        B("CodeBlock", new
        {
            Code            = V("# 1. Install the dotnet template\ndotnet new install BrixCMS.Open.Templates\n\n# 2. Create a new project\ndotnet new brixcms -n MyWebsite\ncd MyWebsite\n\n# 3. Run\ndotnet run\n# → http://localhost:5000\n# → Admin panel: /Manager"),
            Language        = V("bash"),
            Theme           = V("dracula"),
            Title           = V("Quick start"),
            ShowLineNumbers = V("false"),
            ShowCopyButton  = V("true"),
            FontSize        = V("14px"),
            BorderRadius    = V("12px"),
        });

        B("SpacerBlock", new { Height = V("2rem"), BackgroundColor = V(SURFACE) });

        B("CTABannerBlock", new
        {
            Title            = V("Want the full platform?"),
            TitleColor       = V(TEXT),
            TitleSize        = V("2rem"),
            Subtitle         = V("Multi-tenant · White label · BYOK (Gemini, DeepSeek, Mistral) · Stripe · Figma import"),
            SubtitleColor    = V(TEXT2),
            Btn1Text         = V("See BrixCMS Pro →"),
            Btn1Url          = V("/pro"),
            Btn1BgColor      = V(ACCENT),
            Btn1TextColor    = V("#ffffff"),
            Btn2Text         = V("View on GitHub"),
            Btn2Url          = V("https://github.com/Learsi23/BrixCMS"),
            Btn2Color        = V(BORDER),
            BackgroundColor  = V(SURFACE),
            BackgroundColor2 = V(BG),
            PaddingY         = V("5rem"),
            TextAlign        = V("center"),
        });

        db.Blocks.AddRange(blocks);
        db.SaveChanges();
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  PAGE 3 — PRO  (slug: "pro")
    // ═══════════════════════════════════════════════════════════════════════

    private static void SeedProPage(BrixDbContext db)
    {
        var id   = Guid.NewGuid();
        var page = new Page
        {
            Id              = id,
            Title           = "Pro",
            Slug            = "pro",
            IsPublished     = true,
            IsSeed          = true,
            PublishedAt     = DateTime.UtcNow,
            SortOrder       = 2,
            PageType        = "standard",
            MetaDescription = "BrixCMS Pro — multi-tenant, white label, BYOK AI (Gemini, DeepSeek, Mistral, Ollama), Stripe e-commerce, Figma import.",
            JsonData        = JsonSerializer.Serialize(new { BackgroundColor = new { Value = BG } }),
        };
        db.Pages.Add(page);

        int sort = 0;
        var blocks = new List<Block>();
        Block B(string type, object fields, Guid? parentId = null)
        {
            var b = MakeBlock(id, type, fields, sort++, parentId);
            blocks.Add(b);
            return b;
        }

        // ── Hero ──────────────────────────────────────────────────────────────
        B("HeroBlock", new
        {
            Title              = V("BrixCMS Pro"),
            TitleColor         = V(TEXT),
            TitleSize          = V("3.5rem"),
            Subtitle           = V("Multi-tenant · White label · BYOK AI · Stripe · Figma import. The full CMS platform for agencies and professional developers."),
            SubtitleColor      = V(TEXT2),
            SubtitleSize       = V("1.15rem"),
            BackgroundColor    = V(BG),
            BackgroundGradient = V("radial-gradient(ellipse 80% 55% at 60% 0%, rgba(91,110,245,0.13) 0%, transparent 70%)"),
            OverlayOpacity     = V("0.0"),
            Height             = V("half-screen"),
            TextAlign          = V("center"),
            ShowGridPattern    = V("true"),
            GridPatternColor   = V(BORDER),
            ButtonText         = V("Get BrixCMS Pro →"),
            ButtonUrl          = V("https://github.com/Learsi23/BrixCMS"),
            ButtonColor        = V(ACCENT),
            ButtonTextColor    = V("#ffffff"),
        });

        // ── Stats ─────────────────────────────────────────────────────────────
        B("StatsBlock", new
        {
            Title           = V(""),
            Subtitle        = V(""),
            Stat1Number     = V("4"),
            Stat1Label      = V("AI providers — Gemini · DeepSeek · Mistral · Ollama"),
            Stat1Icon       = V("fas fa-brain"),
            Stat2Number     = V("∞"),
            Stat2Label      = V("Client sites — multi-tenant"),
            Stat2Icon       = V("fas fa-sitemap"),
            Stat3Number     = V("1%"),
            Stat3Label      = V("Transaction fee — lowest in class"),
            Stat3Icon       = V("fas fa-shopping-cart"),
            Stat4Number     = V("White"),
            Stat4Label      = V("Label — your brand, your product"),
            Stat4Icon       = V("fas fa-tag"),
            NumberColor     = V(ACCENT),
            LabelColor      = V(TEXT2),
            BackgroundColor = V(SURFACE),
            CardBgColor     = V(SURFACE2),
            PaddingY        = V("3rem"),
        });

        // ── Pro Features ──────────────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("Everything in Open, plus:"),
            TitleColor     = V(TEXT),
            TitleSize      = V("2.2rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("center"),
            BackgroundColor = V(BG),
            Padding        = V("4rem 1.5rem 1.5rem"),
        });

        var proGrid = B("GridColumn", new
        {
            MaxColumns      = V("2"),
            Gap             = V("gap-5"),
            PaddingY        = V("1rem"),
            PaddingX        = V("1.5rem"),
            BackgroundColor = V(BG),
        });

        foreach (var (icon, color, title, text) in new[]
        {
            ("fas fa-sitemap", SUCCESS,
             "Multi-tenant management",
             "Manage unlimited client websites from a single admin panel. Each site is fully isolated — own pages, blocks, media and settings. Perfect for agencies running 5–50+ client sites."),
            ("fas fa-tag", ACCENT,
             "White label",
             "Replace all BrixCMS branding with your own. Custom logo, color scheme and domain. Your clients see your product, not ours."),
            ("fas fa-key", WARNING,
             "BYOK — Bring Your Own Key",
             "Connect Gemini, DeepSeek, Mistral or Ollama. You pay AI providers directly at their official rates — no markup from us. Keys are stored AES-256-GCM encrypted."),
            ("fab fa-figma", "#7C5CBF",
             "Figma → CMS import",
             "AI reads your Figma frames, interprets typography, layout and color palette, then builds the corresponding blocks in the editor automatically."),
            ("fas fa-shopping-cart", SUCCESS,
             "Stripe e-commerce — 1% fee",
             "Sell products directly from any page with Stripe Connect. BrixCMS Pro charges 1% per transaction — half of Webflow's 2%. On €10,000/mo in sales you save €100."),
            ("fas fa-headset", ACCENT,
             "Priority support + SLA",
             "Direct access to the BrixCMS team via dedicated channel. Guaranteed response times. On-boarding session included on all plans."),
        })
        {
            B("IconCardBlock", new
            {
                LeftIconClass   = V(icon),
                LeftIconColor   = V(color),
                LeftIconFaSize  = V("1.75rem"),
                IconPosition    = V("left"),
                TextAlign       = V("left"),
                Title           = V(title),
                TitleColor      = V(TEXT),
                TitleSize       = V("1.05rem"),
                Text            = V(text),
                TextColor       = V(TEXT2),
                BackgroundColor = V(SURFACE),
                BorderColor     = V(BORDER),
                BorderWidth     = V("1px"),
                BorderRadius    = V("12px"),
                Padding         = V("1.5rem"),
            }, proGrid.Id);
        }

        // ── Comparison Table ──────────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("Open vs Pro — at a glance"),
            TitleColor     = V(TEXT),
            TitleSize      = V("2rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("center"),
            BackgroundColor = V(SURFACE),
            Padding        = V("4rem 1.5rem 1.5rem"),
        });

        B("TableBlock", new
        {
            Caption                = V(""),
            Style                  = V("striped"),
            HeaderBackground       = V(SURFACE2),
            HeaderColor            = V(TEXT),
            CellColor              = V(TEXT),
            RowBackground          = V(SURFACE),
            RowAlternateBackground = V(BG),
            BorderColor            = V(BORDER),
            CellPadding            = V("14px"),
            TextAlign              = V("left"),
            MobileScroll           = V("true"),
            TableData              = V(
                "Feature | Open (free · MIT) | Pro (commercial)\n" +
                "Visual page editor | ✅ Yes | ✅ Yes\n" +
                "40+ pre-built blocks | ✅ Yes | ✅ Yes\n" +
                "SQLite database | ✅ Yes | ✅ Yes\n" +
                "Admin panel + 2FA | ✅ Yes | ✅ Yes\n" +
                "Media manager | ✅ Yes | ✅ Yes\n" +
                "Self-hosted | ✅ Yes | ✅ Yes\n" +
                "AI chatbot (Ollama) | ✅ Local & private | ✅ Local & private\n" +
                "AI — Gemini / DeepSeek / Mistral | ❌ Not included | ✅ BYOK (your keys)\n" +
                "Multi-tenant panel | ❌ Single site | ✅ Unlimited sites\n" +
                "White label | ❌ Not included | ✅ Full custom brand\n" +
                "E-commerce (Stripe) | ❌ Not included | ✅ 1% fee\n" +
                "Figma → CMS import | ❌ Not included | ✅ AI-powered\n" +
                "Support | Community (GitHub) | ✅ Direct + SLA\n" +
                "License | MIT (free forever) | Commercial"
            ),
        });

        // ── FAQ ───────────────────────────────────────────────────────────────
        B("TextBlock", new
        {
            Title          = V("Frequently asked questions"),
            TitleColor     = V(TEXT),
            TitleSize      = V("2rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("center"),
            BackgroundColor = V(BG),
            Padding        = V("4rem 1.5rem 0.5rem"),
        });

        var faq = B("FAQBlock", new
        {
            Style            = V("bordered"),
            BackgroundColor  = V(BG),
            Title            = V(""),
            TitleColor       = V(TEXT),
            TitleAlign       = V("center"),
            Description      = V(""),
            DescriptionColor = V(TEXT2),
            QuestionColor    = V(TEXT),
            AnswerColor      = V(TEXT2),
            IconColor        = V(ACCENT),
            BorderColor      = V(BORDER),
            PaddingY         = V("1rem"),
        });

        foreach (var (q, a) in new[]
        {
            ("Is BrixCMS.Open truly free?",
             "Yes. BrixCMS.Open is MIT licensed — free forever, including for commercial projects. You can fork it, modify it and host it without any licensing fee."),
            ("What does BYOK mean?",
             "Bring Your Own Key. In Pro you connect your own API keys for Gemini, DeepSeek or Mistral. You pay those providers directly at their official published rates. BrixCMS Pro never adds a markup on AI usage."),
            ("Can I run AI without an API key?",
             "Yes — in both Open and Pro. Ollama runs locally on your server. Pull any supported model (Llama 3, Mistral, Phi-3, Gemma…) and the chatbot works entirely offline — no API key, no cost."),
            ("What is multi-tenant?",
             "Pro gives you a single admin panel to manage multiple independent websites. Each client site has its own pages, blocks, media and settings — perfect for agencies handling many clients from one place."),
            ("How does white label work?",
             "In Pro you can replace all BrixCMS branding with your own — logo, colors, domain and product name. Your clients see your CMS, not ours."),
            ("How does Figma import work?",
             "In Pro, the AI reads your Figma file, detects headings, body text, images and layout structure, then automatically builds the matching blocks in the BrixCMS editor."),
            ("What is the difference between Open and Pro?",
             "Open is a complete, self-hosted CMS for individuals and developers — free forever. Pro adds the tools agencies and teams need: multi-tenant management, white label, multiple AI providers (BYOK), Stripe e-commerce and Figma import."),
        })
        {
            B("AccordionItemBlock", new
            {
                Question = V(q),
                Answer   = V(a),
            }, faq.Id);
        }

        // ── Final CTA ─────────────────────────────────────────────────────────
        B("CTABannerBlock", new
        {
            Title            = V("Ready to upgrade?"),
            TitleColor       = V(TEXT),
            TitleSize        = V("2.5rem"),
            Subtitle         = V("BrixCMS.Open is always free. Go Pro when you need the full platform."),
            SubtitleColor    = V(TEXT2),
            Btn1Text         = V("Get BrixCMS Pro →"),
            Btn1Url          = V("https://github.com/Learsi23/BrixCMS"),
            Btn1BgColor      = V(ACCENT),
            Btn1TextColor    = V("#ffffff"),
            Btn2Text         = V("← Back to Open"),
            Btn2Url          = V("/"),
            Btn2Color        = V(BORDER),
            BackgroundColor  = V(SURFACE),
            BackgroundColor2 = V(BG),
            PaddingY         = V("6rem"),
            TextAlign        = V("center"),
        });

        db.Blocks.AddRange(blocks);
        db.SaveChanges();
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  SITE CONFIG — dark navbar + footer
    // ═══════════════════════════════════════════════════════════════════════

    private static void SeedSiteConfig(BrixDbContext db)
    {
        if (db.SiteConfig.Any(c => c.Key == "site"))
            return;

        var config = new
        {
            navbar = new
            {
                backgroundColor = BG,
                textColor       = TEXT,
                logo            = "/images/logo-menu.png",
                logoAltText     = "BrixCMS",
                logoWidth       = "120px",
                logoLink        = "/",
                isSticky        = true,
                hasShadow       = false,
                paddingVertical = "py-3",
                menuItems       = new object[]
                {
                    new { customText = "Features", customUrl = "/features",                              isCustomUrl = true, pageSlug = "", openInNewTab = false },
                    new { customText = "Pro",      customUrl = "/pro",                                   isCustomUrl = true, pageSlug = "", openInNewTab = false },
                    new { customText = "GitHub",   customUrl = "https://github.com/Learsi23/BrixCMS",   isCustomUrl = true, pageSlug = "", openInNewTab = true  },
                    new { customText = "Admin",    customUrl = "/Manager/login",                         isCustomUrl = true, pageSlug = "", openInNewTab = false },
                },
            },
            footer = new
            {
                backgroundColor        = SURFACE,
                textColor              = TEXT2,
                logo                   = "/images/logo-menu.png",
                logoAltText            = "BrixCMS",
                logoWidth              = "100px",
                logoPosition           = "left",
                showPagesColumn        = false,
                pagesColumnTitle       = "Pages",
                pages                  = Array.Empty<object>(),
                showSocialMediaColumn  = true,
                socialMediaColumnTitle = "Links",
                socialMedia            = new object[]
                {
                    new { platform = "github",  url = "https://github.com/Learsi23/BrixCMS", iconClass = "fab fa-github"  },
                    new { platform = "youtube", url = "https://www.youtube.com/@BrixCMS",     iconClass = "fab fa-youtube" },
                },
                showCopyrightRow   = true,
                companyName        = "BrixCMS",
                companyNumber      = "",
                copyrightText      = "MIT License — free forever",
                showHorizontalLine = true,
                paddingVertical    = "py-8",
                columnsGap         = "gap-8",
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
