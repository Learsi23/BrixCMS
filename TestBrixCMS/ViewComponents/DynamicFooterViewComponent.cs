using TestBrixCMS.Data;
using TestBrixCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TestBrixCMS.ViewComponents;

public class DynamicFooterViewComponent : ViewComponent
{
    private readonly BrixDbContext _db;

    public DynamicFooterViewComponent(BrixDbContext db)
    {
        _db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var config = await GetFooterSettingsAsync();
        return View(config);
    }

    private async Task<FooterSettings> GetFooterSettingsAsync()
    {
        var siteConfig = await _db.SiteConfig.FirstOrDefaultAsync(c => c.Key == "site");
        if (siteConfig == null || string.IsNullOrEmpty(siteConfig.JsonData))
        {
            return new FooterSettings();
        }

        try
        {
            var settings = JsonSerializer.Deserialize<SiteSettings>(siteConfig.JsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return settings?.Footer ?? new FooterSettings();
        }
        catch
        {
            return new FooterSettings();
        }
    }
}
