using TestBrixCMS.Areas.Manager.Filters;
using TestBrixCMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TestBrixCMS.Areas.Manager.Controllers;

[Area("Manager")]
[RequireAdminLogin]
public class DemoSeederController : Controller
{
    private readonly BrixDbContext _db;
    public DemoSeederController(BrixDbContext db) => _db = db;

    static object V(string? v) => new { Value = v ?? "" };

    Block MakeBlock(Guid pageId, string type, object fields, int order, Guid? parentId = null) => new()
    {
        Id = Guid.NewGuid(),
        PageId = pageId,
        ParentId = parentId,
        Type = type,
        SortOrder = order,
        JsonData = JsonSerializer.Serialize(fields)
    };

    [HttpGet]
    public async Task<IActionResult> SeedBlocksDemo()
    {
        var existing = await _db.Pages.FirstOrDefaultAsync(p => p.Slug == "blocks-demo");
        if (existing != null)
        {
            var existingBlocks = await _db.Blocks.Where(b => b.PageId == existing.Id).ToListAsync();
            _db.Blocks.RemoveRange(existingBlocks);
            _db.Pages.Remove(existing);
            await _db.SaveChangesAsync();
        }

        var pageId = Guid.NewGuid();
        var page = new Page
        {
            Id = pageId,
            Title = "Blocks Demo - New Features",
            Slug = "blocks-demo",
            IsPublished = true,
            PublishedAt = DateTime.UtcNow,
            SortOrder = 999,
            MetaDescription = "All new BrixCMS blocks in one page"
        };
        _db.Pages.Add(page);

        var blocks = new List<Block>();
        int rootOrder = 0;

        // Hero Header - use FullColumnBlock
        blocks.Add(MakeBlock(pageId, "FullColumnBlock", new
        {
            Title = new { Value = "BrixCMS - All New Blocks Demo" },
            TitleColor = new { Value = "#ffffff" },
            SubTitle = new { Value = "One page showing all 10 new blocks" },
            SubTitleColor = new { Value = "#94a3b8" },
            Description = new { Value = "Scroll down to explore each block. Every section uses a different new block with real configuration." },
            BackgroundColor = new { Value = "#0f172a" },
            TextColor = new { Value = "#ffffff" },
            TitleAlignment = new { Value = "center" },
            SubTitleAlignment = new { Value = "center" },
            HeaderTextAlign = new { Value = "center" },
            PaddingY = new { Value = "4rem" },
        }, rootOrder++));

        // 1. ColumnBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("1. ColumnBlock"),
            TitleColor = V("#1e293b"),
            TitleWeight = V("700"),
            Body = V("Simple column container with light blue background, border radius and shadow."),
            BodyColor = V("#475569"),
            BackgroundColor = V("#dbeafe"),
            PaddingY = V("2rem"),
            PaddingX = V("1.5rem"),
            BorderRadius = V("0.75rem"),
            Shadow = V("md"),
            MarginBottom = V("1rem"),
            TitleAlignment = V("left"),
        }, rootOrder++));

        // 2. FAQBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("2. FAQBlock"),
            TitleColor = V("#1e293b"),
            Body = V("Specialized FAQ block. Styles: Cards, Bordered, List. Has + / - icon to expand answers."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
        }, rootOrder++));

        // FAQ Block with items
        var faqId = Guid.NewGuid();
        blocks.Add(new Block { Id = faqId, PageId = pageId, Type = "FAQBlock", SortOrder = rootOrder++,
            JsonData = JsonSerializer.Serialize(new {
                Style = V("cards"),
                Columns = V("1"),
                Title = V("FAQ Block - example"),
                TitleColor = V("#1e293b"),
                TitleAlign = V("center"),
                Description = V("Click on a question to expand it. Supports 1 or 2 columns."),
                DescriptionColor = V("#64748b"),
                QuestionColor = V("#1e293b"),
                AnswerColor = V("#475569"),
                IconColor = V("#5B6EF5"),
            })
        });

        // FAQ Items
        var faqItems = new[] {
            new { Q = "What is FAQBlock?", A = "FAQBlock is a specialized block for frequently asked questions. It supports different styles like cards, bordered, or list." },
            new { Q = "How many columns does it support?", A = "FAQBlock supports either 1 or 2 columns depending on your layout needs." },
            new { Q = "Can I customize the styling?", A = "Yes! You can customize colors, fonts, icons, and more from the block settings." }
        };
        for (int i = 0; i < faqItems.Length; i++)
        {
            blocks.Add(MakeBlock(pageId, "AccordionItemBlock", new {
                Question = new { Value = faqItems[i].Q },
                Answer = new { Value = faqItems[i].A },
            }, i, faqId));
        }

        // 3. Before/After intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("3. Before/After Image Block"),
            TitleColor = V("#1e293b"),
            Body = V("Comparison slider. Drag to see before vs after. Ideal for transformations, products, treatments."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
        }, rootOrder++));

        // 4. AudioBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("4. AudioBlock"),
            TitleColor = V("#1e293b"),
            Body = V("Audio player. Styles: Minimal, Card, Full. Supports cover, artist, description."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
        }, rootOrder++));

        // 5. TableBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("5. TableBlock"),
            TitleColor = V("#1e293b"),
            Body = V("Data table from JSON. Styles: Striped, Bordered, Default, Minimal."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
        }, rootOrder++));

        // Real Table Example
        blocks.Add(MakeBlock(pageId, "TableBlock", new
        {
            Caption = V("Pricing Plans"),
            Style = V("striped"),
            HeaderBackground = V("#1e293b"),
            HeaderColor = V("#ffffff"),
            RowBackground = V("#ffffff"),
            RowAlternateBackground = V("#f8fafc"),
            BorderColor = V("#e2e8f0"),
            CellPadding = V("16px"),
            TextAlign = V("center"),
            MobileScroll = V("true"),
            TableData = V(JsonSerializer.Serialize(new {
                headers = new[] { "Block", "Type", "Children", "Use cases" },
                rows = new[] {
                    new[] { "ColumnBlock", "Layout", "Yes", "Cards in row, 2-4 columns" },
                    new[] { "FAQBlock", "Interactive", "Yes", "Frequently asked questions" },
                    new[] { "BeforeAfterBlock", "Media", "No", "Image comparison" },
                    new[] { "AudioBlock", "Media", "No", "Podcast, music, effects" },
                    new[] { "TableBlock", "Content", "No", "Data, prices, comparisons" },
                }
            })),
        }, rootOrder++));

        // 6. FeatureListBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("6. FeatureListBlock"),
            TitleColor = V("#1e293b"),
            Body = V("Features list with icons. Grid 1-4 columns. Icon + title + description per item."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
            MarginTop = V("2rem"),
        }, rootOrder++));

        // Feature List with items
        var featureListId = Guid.NewGuid();
        blocks.Add(new Block { Id = featureListId, PageId = pageId, Type = "FeatureListBlock", SortOrder = rootOrder++,
            JsonData = JsonSerializer.Serialize(new {
                Title = V("Why BrixCMS?"),
                TitleColor = V("#1e293b"),
                TitleAlign = V("center"),
                Columns = V("2"),
                Gap = V("1.5rem"),
                ItemIconColor = V("#10b981"),
                BackgroundColor = V("#f8fafc"),
                PaddingY = V("2rem"),
            })
        });

        var featureItems = new[] {
            new { T = "BYOK without commission", D = "Your key, your cost, 0% markup." },
            new { T = "Multi-tenant", D = "Manage all your clients from one dashboard." },
            new { T = "White Label", D = "Your brand, your platform, your rules." },
            new { T = "Ollama / Local AI", D = "Run 100% private models on your server." }
        };
        for (int i = 0; i < featureItems.Length; i++)
        {
            blocks.Add(MakeBlock(pageId, "TextBlock", new
            {
                Title = new { Value = "✓" },
                Description = new { Value = featureItems[i].T + " - " + featureItems[i].D },
                TitleColor = new { Value = "#10b981" },
            }, i, featureListId));
        }

        // 7. CookieBannerBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("7. CookieBannerBlock"),
            TitleColor = V("#1e293b"),
            Body = V("GDPR banner. Fixed on bottom/top. Saves consent to localStorage. Accept/Decline buttons."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
            MarginTop = V("2rem"),
        }, rootOrder++));

        // Cookie Banner (at end)
        blocks.Add(MakeBlock(pageId, "CookieBannerBlock", new
        {
            Position = V("bottom"),
            BackgroundColor = V("#1e293b"),
            TextColor = V("#ffffff"),
            Title = V("We use cookies"),
            Message = V("This site uses cookies to improve your experience."),
            AcceptButtonText = V("Accept"),
            DeclineButtonText = V("Decline"),
            AcceptButtonColor = V("#10b981"),
            DeclineButtonColor = V("#475569"),
            ButtonTextColor = V("#ffffff"),
            BorderRadius = V("8px"),
            ZIndex = V("9999"),
        }, rootOrder++));

        // 8. LottieBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("8. LottieBlock"),
            TitleColor = V("#1e293b"),
            Body = V("Extremely lightweight JSON animations. Auto-play, loop, speed configurable."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
            MarginTop = V("2rem"),
        }, rootOrder++));

        // 9. CodeBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("9. CodeBlock"),
            TitleColor = V("#1e293b"),
            Body = V("Code with syntax highlighting. Themes: Dark, GitHub, Monokai, Dracula. Line numbers + copy button."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
            MarginTop = V("2rem"),
        }, rootOrder++));

        // Real Code Example
        blocks.Add(MakeBlock(pageId, "CodeBlock", new
        {
            Code = V("// BrixCMS CodeBlock - Dracula Theme\n// Syntax highlighting without dependencies\n\ninterface BrixConfig {\n  provider: \"openai\" | \"gemini\" | \"claude\" | \"ollama\";\n  apiKey:   string;\n  model:    string;\n  local:    boolean;\n}\n\nconst config: BrixConfig = {\n  provider: \"openai\",\n  apiKey:   process.env.OPENAI_KEY ?? \"\",\n  model:    \"gpt-4o-mini\",\n  local:    false,\n};"),
            Language = V("typescript"),
            Theme = V("dracula"),
            Title = V("api-key.ts"),
            ShowLineNumbers = V("true"),
            ShowCopyButton = V("true"),
            FontSize = V("13px"),
            BorderRadius = V("8px"),
        }, rootOrder++));

        // 10. QRCodeBlock intro
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("10. QRCodeBlock"),
            TitleColor = V("#1e293b"),
            Body = V("QR generator from URL, WiFi, vCard, Email. Configurable sizes, colors, borders."),
            BodyColor = V("#475569"),
            MarginBottom = V("1rem"),
            MarginTop = V("2rem"),
        }, rootOrder++));

        // Real QR Example
        blocks.Add(MakeBlock(pageId, "QRCodeBlock", new
        {
            Content = V("https://brixcms.com"),
            ContentType = V("url"),
            Size = V("180"),
            ForegroundColor = V("#1e293b"),
            BackgroundColor = V("#ffffff"),
            Style = V("square"),
            IncludeLabel = V("true"),
            Label = V("Scan to open BrixCMS"),
            LabelColor = V("#64748b"),
            BorderRadius = V("8px"),
            Align = V("center"),
        }, rootOrder++));

        // Configuration Notes
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("Configuration Notes"),
            TitleColor = V("#1e293b"),
            TitleWeight = V("700"),
            TitleAlignment = V("center"),
            MarginTop = V("3rem"),
            MarginBottom = V("1rem"),
        }, rootOrder++));

        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("AudioBlock"),
            TitleColor = V("#64748b"),
            Body = V("→ Requires AudioUrl (path to .mp3/.ogg/.wav uploaded to /uploads/). If no AudioUrl, block shows a placeholder."),
            BodyColor = V("#475569"),
        }, rootOrder++));

        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("BeforeAfterBlock"),
            TitleColor = V("#64748b"),
            Body = V("→ Requires BeforeImage and AfterImage (both with real image). Without images shows configuration placeholder. Use external URLs or upload to /uploads/."),
            BodyColor = V("#475569"),
        }, rootOrder++));

        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("LottieBlock"),
            TitleColor = V("#64748b"),
            Body = V("→ Requires LottieUrl (path to .json animation file). Upload .json to /uploads/ and reference /uploads/animation.json. Compatible with standard and DotLottie files."),
            BodyColor = V("#475569"),
        }, rootOrder++));

        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("CookieBannerBlock"),
            TitleColor = V("#64748b"),
            Body = V("→ Renders FIXED on page (position: fixed). Place it ALWAYS at the end of the block array. Max 1 per page. Uses localStorage to remember user choice."),
            BodyColor = V("#475569"),
        }, rootOrder++));

        // End
        blocks.Add(MakeBlock(pageId, "TextBlock", new
        {
            Title = V("--- End of Demo ---"),
            TitleColor = V("#94a3b8"),
            TitleAlignment = V("center"),
            MarginY = V("4rem"),
        }, rootOrder++));

        _db.Blocks.AddRange(blocks);
        await _db.SaveChangesAsync();

        return Content($"✅ Blocks Demo created with {blocks.Count} blocks. Visit <a href='/blocks-demo'>/blocks-demo</a> to see it.", "text/html");
    }
}
