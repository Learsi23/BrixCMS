using TestBrixCMS.Data;
using TestBrixCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TestBrixCMS.ViewComponents;

public class DynamicNavbarViewComponent : ViewComponent
{
    private readonly BrixDbContext _db;

    public DynamicNavbarViewComponent(BrixDbContext db)
    {
        _db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var config = await GetNavbarSettingsAsync();
        return View(config);
    }

    private async Task<NavbarSettings> GetNavbarSettingsAsync()
    {
        var siteConfig = await _db.SiteConfig.FirstOrDefaultAsync(c => c.Key == "site");
        if (siteConfig == null || string.IsNullOrEmpty(siteConfig.JsonData))
        {
            return new NavbarSettings();
        }

        try
        {
            var settings = JsonSerializer.Deserialize<SiteSettings>(siteConfig.JsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return settings?.Navbar ?? new NavbarSettings();
        }
        catch
        {
            return new NavbarSettings();
        }
    }
}
