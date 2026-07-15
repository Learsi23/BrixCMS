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
            MetaDescription = "BrixCMS.Open — free, self-hosted, open-source .NET 10 CMS. 50+ blocks, Ollama AI chatbot, SQLite, MIT license.",
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
            Subtitle           = V("BrixCMS.Open is a free, open-source .NET 10 block-based CMS with 50+ visual blocks, Ollama AI chatbot, and zero configuration."),
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
            Stat1Number     = V("50+"),
            Stat1Label      = V("Pre-built blocks"),
            Stat1Icon       = V("fas fa-th-large"),
            Stat2Number     = V("MIT"),
            Stat2Label      = V("Open source license"),
            Stat2Icon       = V("fas fa-code-branch"),
            Stat3Number     = V(".NET 10"),
            Stat3Label      = V("Blazor Server + MVC"),
            Stat3Icon       = V("fas fa-bolt"),
            Stat4Number     = V("AI"),
            Stat4Label      = V("Ollama local · Gemini cloud"),
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
             "50+ pre-built blocks",
             "Hero, Stats, FAQ, Testimonials, Gallery, Map, Countdown, Timeline, Tabs, Team, Table, QR, Code and more. Add your own in minutes."),
            ("fas fa-robot", SUCCESS,
             "AI chatbot — Ollama local or Gemini cloud",
             "Drop PDFs in /wwwroot/Data for an instant knowledge-base chatbot. Use Ollama locally (free, private, no API key) or connect a free Gemini API key for cloud AI. Switch between them in the admin panel — no code changes needed."),
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
            Description = V("Open the page editor, drag & drop 50+ blocks, configure colors, text and images. Publish with one click. Your site is live."),
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
            Subtitle       = V("BrixCMS.Open is free forever. BrixCMS Pro adds multi-tenant management, white label branding, additional AI providers, Stripe e-commerce and Figma import."),
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
            Btn2Text         = V("Pro — Coming Soon →"),
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
            MetaDescription = "BrixCMS.Open features — 50+ blocks, Ollama AI chatbot, visual page editor, SQLite, MIT license.",
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
            Title              = V("50+ blocks. Zero config."),
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
            Title          = V("AI — local or cloud, your choice"),
            TitleColor     = V(TEXT),
            TitleSize      = V("1.9rem"),
            TitleWeight    = V("800"),
            TitleAlignment = V("left"),
            BackgroundColor = V(BG),
            Padding        = V("4rem 1.5rem 0.5rem"),
        });

        var aiGrid = B("GridColumn", new
        {
            MaxColumns      = V("3"),
            Gap             = V("gap-5"),
            PaddingY        = V("1rem"),
            PaddingX        = V("1.5rem"),
            BackgroundColor = V(BG),
        });

        B("IconCardBlock", new
        {
            LeftIconClass   = V("fas fa-server"),
            LeftIconColor   = V(SUCCESS),
            LeftIconFaSize  = V("1.75rem"),
            IconPosition    = V("left"),
            TextAlign       = V("left"),
            Title           = V("Ollama — local, free, private"),
            TitleColor      = V(TEXT),
            TitleSize       = V("1.05rem"),
            Text            = V("Pull any model (Llama 3, Phi-3, Gemma, Mistral…) and run the chatbot 100% on your server. No API key, no cost, no data leaving your infrastructure. Default when no cloud key is configured."),
            TextColor       = V(TEXT2),
            BackgroundColor = V(SURFACE),
            BorderColor     = V(BORDER),
            BorderWidth     = V("1px"),
            BorderRadius    = V("12px"),
            Padding         = V("1.5rem"),
        }, aiGrid.Id);

        B("IconCardBlock", new
        {
            LeftIconClass   = V("fas fa-cloud"),
            LeftIconColor   = V(ACCENT),
            LeftIconFaSize  = V("1.75rem"),
            IconPosition    = V("left"),
            TextAlign       = V("left"),
            Title           = V("Gemini — cloud, free tier"),
            TitleColor      = V(TEXT),
            TitleSize       = V("1.05rem"),
            Text            = V("Paste a free Google AI Studio key in Admin → Chatbot & Security. Gemini 2.5 Flash Lite is free (60 req/min). Your key is stored AES-256-GCM encrypted. Falls back to Ollama automatically if unreachable."),
            TextColor       = V(TEXT2),
            BackgroundColor = V(SURFACE),
            BorderColor     = V(BORDER),
            BorderWidth     = V("1px"),
            BorderRadius    = V("12px"),
            Padding         = V("1.5rem"),
        }, aiGrid.Id);

        B("IconCardBlock", new
        {
            LeftIconClass   = V("fas fa-file-pdf"),
            LeftIconColor   = V(WARNING),
            LeftIconFaSize  = V("1.75rem"),
            IconPosition    = V("left"),
            TextAlign       = V("left"),
            Title           = V("PDF knowledge base"),
            TitleColor      = V(TEXT),
            TitleSize       = V("1.05rem"),
            Text            = V("Drop any PDF into /wwwroot/Data (or upload via the admin panel). On startup BrixCMS ingests it into an in-memory vector store. The ChatBlock answers questions grounded in your documents — with source citations. Works with both Ollama and Gemini."),
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
            // Kept in sync with Extensions/BlockRegistrationExtensions.cs — every standalone
            // (non-child) registered block, using each block's own [BlockType] description.
            ("fas fa-th", "GridColumn", "Flexible CSS grid with configurable rows and columns. Each row can have different column counts."),
            ("fas fa-arrows-alt-v", "SpacerBlock", "Adds vertical whitespace between blocks. Set exact height in px, rem, or vh."),
            ("fas fa-grip-lines", "DividerBlock", "Horizontal separator line with configurable color, thickness, and style."),
            ("fas fa-bullhorn", "BannerBlock", "Announcement or notification bar: free shipping, active discount, new feature. Can be closed."),
            ("fas fa-columns", "FullColumnBlock", "Multi-column layout container. Add any blocks inside each column for flexible page layouts."),
            ("fas fa-question-circle", "FAQBlock", "Specialized FAQ block with structured questions and answers. Different from accordion - optimized for Q&A content."),
            ("fas fa-columns", "BeforeAfterBlock", "Image comparison slider. Perfect for showing transformations, before/after results, or product comparisons."),
            ("fas fa-headphones", "AudioBlock", "Audio player for podcasts, music, or voice recordings. Supports custom cover image and episode info."),
            ("fas fa-table", "TableBlock", "Simple data table. Different from comparison table - optimized for displaying structured data."),
            ("fas fa-list-check", "FeatureListBlock", "List of features with icons. Perfect for showing features, benefits, or checklists."),
            ("fas fa-cookie", "CookieBannerBlock", "GDPR cookie consent banner. Display privacy notice and consent buttons."),
            ("fas fa-envelope-open-text", "NewsletterBlock", "Email subscription form with a title, subtitle, and customisable call-to-action button."),
            ("fas fa-film", "LottieBlock", "Lottie animation. Lightweight JSON animations for engaging visual content."),
            ("fas fa-code", "CodeBlock", "Syntax highlighted code block. Perfect for code snippets, API examples, and tutorials."),
            ("fas fa-qrcode", "QRCodeBlock", "QR code generator. Perfect for linking to URLs, payments, WiFi access, or contact info."),
            ("fas fa-play-circle", "HeroBlock", "Full-width hero with title, subtitle, and background image. Perfect for page introductions."),
            ("fas fa-film", "HeroFlexibleBlock", "Hero with flexible background: video, image, or color. Multiple buttons support."),
            ("fas fa-font", "TextBlock", "Rich text block with title, subtitle, and body. Full control over size, color, and alignment."),
            ("fab fa-markdown", "MarkdownBlock", "Write content in Markdown with full WYSIWYG editor (TinyMCE). Ideal for articles, docs, and rich text."),
            ("fas fa-image", "ImageBlock", "Single image with configurable size, alt text, border radius, and optional caption."),
            ("fas fa-layer-group", "CardBlock", "Flexible card with image, title, description, icon, and CTA button. Supports vertical, horizontal, and overlay layouts."),
            ("fas fa-id-card", "IconCardBlock", "Card with a large icon, title, and description. Great for features or services sections."),
            ("fas fa-chart-bar", "StatsBlock", "Display key metrics or numbers with labels. Ideal for social proof and achievement sections."),
            ("fas fa-bullhorn", "CTABannerBlock", "Call-to-action banner with headline, description, and a prominent button to drive conversions."),
            ("fas fa-handshake", "LogoStripBlock", "Horizontal row of partner or brand logos. Builds trust and social proof on landing pages."),
            ("fas fa-stream", "TimelineBlock", "Timeline of steps or milestones. Ideal for 'How it works', company history, or process."),
            ("fas fa-quote-left", "TestimonialsBlock", "Carousel of customer reviews with avatar, name, role, and stars."),
            ("fas fa-award", "SocialProofBlock", "Client logos + number of Google or Trustpilot reviews. Builds trust."),
            ("fas fa-users", "TeamBlock", "Grid of team members with photo, name, position, and social media links."),
            ("fas fa-th-large", "FeatureGridBlock", "Display a grid of feature cards with icons. Perfect for services, benefits, or platform features sections."),
            ("fas fa-th", "BrixGridDemoBlock", "Visual demonstration of the modular block system. Shows a grid of draggable blocks."),
            ("fas fa-utensils", "MenuBlock", "Display a restaurant menu with categories, dish names, descriptions, and prices."),
            ("fas fa-clock", "OpeningHoursBlock", "Weekly opening hours table with a live open/closed status indicator."),
            ("fas fa-images", "GalleryBlock", "Image gallery with grid, masonry, or carousel display modes. Supports lightbox on click."),
            ("fas fa-columns", "FlexibleImageTextBlock", "Side-by-side image and text layout. Supports left/right image position, rounded corners, and custom spacing."),
            ("fas fa-play-circle", "VideoBlock", "Embed a YouTube, Vimeo, or direct video URL with autoplay and loop options."),
            ("fas fa-map-marker-alt", "MapBlock", "Embed a Google Maps location by address or coordinates. Configurable height and zoom level."),
            ("fas fa-hand-pointer", "ButtonLinkBlock", "Standalone CTA button with configurable URL, style, color, and size. Supports new tab."),
            ("fas fa-chevron-down", "DropdownBlock", "Collapsible accordion sections. Perfect for FAQs, specifications, or grouped content."),
            ("fas fa-align-left", "TextWithButtonBlock", "Text content paired with a CTA button. Clean layout for announcements, offers, or feature highlights."),
            ("fas fa-list-ul", "AccordionBlock", "Multi-item collapsible accordion. Perfect for FAQs with multiple questions in one block."),
            ("fas fa-hourglass-half", "CountdownBlock", "Countdown timer for offers, launches, or events. Set the target date."),
            ("fas fa-folder-open", "TabsBlock", "Content organized in tabs. Ideal for Description / Specifications / Reviews."),
            ("fas fa-envelope-open-text", "EmailButtonBlock", "Button that triggers an email action (e.g. inquiry, quote request). Sends via your SMTP configuration."),
            ("fas fa-robot", "ChatBlock", "Modern AI chatbot. Use Fullscreen for a DeepSeek/Gemini-style full-page chat, or Embedded for a card-style widget."),
            ("fas fa-comment-dots", "FloatingChatBlock", "A modern floating chat button with glassmorphism panel, animated gradients, and fully customizable colors."),
            ("fas fa-envelope", "ContactFormBlock", "Interactive contact form with email delivery. Fully customizable fields and confirmation message."),
            ("fas fa-image", "StartHeroBlock", "Animated hero with background image, gradient overlay, title, subtitle, and CTA button."),
            ("fas fa-play", "LogoStartBlock", "Hero header with animated logo and sequential text or image elements that slide in from the right."),
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
            Subtitle         = V("Multi-tenant · White label · Additional AI providers (BYOK) · Stripe e-commerce · Figma import"),
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

        // ── Coming Soon Banner ────────────────────────────────────────────────
        B("BannerBlock", new
        {
            Icon            = V("🚀"),
            Text            = V("BrixCMS Pro is currently in development. Join the waitlist to get early access."),
            LinkText        = V(""),
            LinkUrl         = V(""),
            BackgroundColor = V(ACCENT),
            TextColor       = V("#ffffff"),
            Closeable       = V("false"),
        });

        // ── Hero ──────────────────────────────────────────────────────────────
        B("HeroBlock", new
        {
            Title              = V("BrixCMS Pro — Coming Soon"),
            TitleColor         = V(TEXT),
            TitleSize          = V("3.5rem"),
            Subtitle           = V("The full CMS platform for agencies and professional developers is on its way. Multi-tenant · White label · BYOK AI · Stripe · Figma import."),
            SubtitleColor      = V(TEXT2),
            SubtitleSize       = V("1.15rem"),
            BackgroundColor    = V(BG),
            BackgroundGradient = V("radial-gradient(ellipse 80% 55% at 60% 0%, rgba(91,110,245,0.13) 0%, transparent 70%)"),
            OverlayOpacity     = V("0.0"),
            Height             = V("half-screen"),
            TextAlign          = V("center"),
            ShowGridPattern    = V("true"),
            GridPatternColor   = V(BORDER),
            ButtonText         = V("← Back to Open"),
            ButtonUrl          = V("/"),
            ButtonColor        = V(SURFACE2),
            ButtonTextColor    = V(TEXT),
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
                "50+ pre-built blocks | ✅ Yes | ✅ Yes\n" +
                "SQLite database | ✅ Yes | ✅ Yes\n" +
                "Admin panel + 2FA | ✅ Yes | ✅ Yes\n" +
                "Media manager | ✅ Yes | ✅ Yes\n" +
                "Self-hosted | ✅ Yes | ✅ Yes\n" +
                "AI chatbot (Ollama local) | ✅ Local & private | ✅ Local & private\n" +
                "AI — Gemini cloud (free tier) | ✅ BYOK (your key) | ✅ BYOK (your keys)\n" +
                "AI — Additional cloud providers | ❌ Not included | ✅ BYOK (your keys)\n" +
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

        // ── Coming Soon CTA ───────────────────────────────────────────────────
        B("CTABannerBlock", new
        {
            Title            = V("Pro is coming — stay in the loop"),
            TitleColor       = V(TEXT),
            TitleSize        = V("2.5rem"),
            Subtitle         = V("BrixCMS.Open is always free. Pro is in active development — follow the repo to get notified the moment it launches."),
            SubtitleColor    = V(TEXT2),
            Btn1Text         = V("Watch on GitHub →"),
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
                    new { customText = "Features", customUrl = "/features",                            isCustomUrl = true, pageSlug = "", openInNewTab = false, iconClass = "", iconAriaLabel = "" },
                    new { customText = "Pro",      customUrl = "/pro",                                 isCustomUrl = true, pageSlug = "", openInNewTab = false, iconClass = "", iconAriaLabel = "" },
                    new { customText = "",         customUrl = "https://github.com/Learsi23/BrixCMS", isCustomUrl = true, pageSlug = "", openInNewTab = true,  iconClass = "fab fa-github",  iconAriaLabel = "BrixCMS on GitHub"  },
                    new { customText = "",         customUrl = "https://www.youtube.com/@BrixCMS",    isCustomUrl = true, pageSlug = "", openInNewTab = true,  iconClass = "fab fa-youtube", iconAriaLabel = "BrixCMS on YouTube" },
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
