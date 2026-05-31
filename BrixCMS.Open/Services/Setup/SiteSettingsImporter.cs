using BrixCMS.Open.Data;
using BrixCMS.Open.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BrixCMS.Open.Services;

public class SiteSettingsImporter
{
    private readonly BrixDbContext _db;
    private readonly ILogger<SiteSettingsImporter> _logger;

    public SiteSettingsImporter(BrixDbContext db, ILogger<SiteSettingsImporter> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task AddPageToNavAsync(string title, string slug, bool isSubpage = false)
    {
        if (string.IsNullOrWhiteSpace(slug) || isSubpage)
            return;

        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var cfg = await _db.SiteConfig.FirstOrDefaultAsync(c => c.Key == "site");
        var current = new SiteSettings();

        if (cfg != null && !string.IsNullOrEmpty(cfg.JsonData))
        {
            try { current = JsonSerializer.Deserialize<SiteSettings>(cfg.JsonData, opts) ?? new SiteSettings(); }
            catch (Exception ex) { _logger.LogWarning(ex, "SiteConfig JSON unreadable in AddPageToNavAsync."); }
        }

        // First real page publish: clear seed items (those without a pageSlug)
        if (!current.Navbar.MenuItems.Any(m => !string.IsNullOrEmpty(m.PageSlug)))
        {
            current.Navbar.MenuItems.Clear();
            current.Footer.Pages.Clear();
        }

        // Delete seed pages (Features, Pro) and their subpages when a real page is created
        var seedSlugs = new[] { "features", "pro" };
        var seedPages = await _db.Pages.Where(p => seedSlugs.Contains(p.Slug) && !p.ParentId.HasValue).ToListAsync();
        if (seedPages.Count > 0)
        {
            var seedIds = seedPages.Select(p => p.Id).ToList();
            var subpages = await _db.Pages.Where(p => p.ParentId.HasValue && seedIds.Contains(p.ParentId.Value)).ToListAsync();
            foreach (var sub in subpages)
                _logger.LogInformation("Removing seed subpage '{Title}' ({Slug})", sub.Title, sub.Slug);
            _db.Pages.RemoveRange(subpages);
            foreach (var sp in seedPages)
                _logger.LogInformation("Removing seed page '{Title}' ({Slug})", sp.Title, sp.Slug);
            _db.Pages.RemoveRange(seedPages);
        }

        if (!current.Navbar.MenuItems.Any(m => m.PageSlug == slug))
            current.Navbar.MenuItems.Add(new MenuItemConfig { CustomText = title, PageSlug = slug });

        if (!current.Footer.Pages.Any(m => m.PageSlug == slug))
            current.Footer.Pages.Add(new MenuItemConfig { CustomText = title, PageSlug = slug });

        if (cfg == null) { cfg = new SiteConfig { Key = "site" }; _db.SiteConfig.Add(cfg); }
        cfg.JsonData = JsonSerializer.Serialize(current, opts);
        await _db.SaveChangesAsync();
    }

    public async Task RemovePageFromNavAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return;

        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var cfg = await _db.SiteConfig.FirstOrDefaultAsync(c => c.Key == "site");
        if (cfg == null || string.IsNullOrEmpty(cfg.JsonData)) return;

        SiteSettings current;
        try { current = JsonSerializer.Deserialize<SiteSettings>(cfg.JsonData, opts) ?? new SiteSettings(); }
        catch { return; }

        var changed = current.Navbar.MenuItems.RemoveAll(m => m.PageSlug == slug) > 0;
        changed |= current.Footer.Pages.RemoveAll(m => m.PageSlug == slug) > 0;

        if (changed)
        {
            cfg.JsonData = JsonSerializer.Serialize(current, opts);
            await _db.SaveChangesAsync();
        }
    }
}
