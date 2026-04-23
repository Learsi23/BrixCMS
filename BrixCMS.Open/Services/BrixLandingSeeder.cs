using BrixCMS.Open.Data;
using System.Text.Json;

namespace BrixCMS.Open.Services;

/// <summary>
/// Seeds the BrixCMS landing page in the DB if it doesn't exist.
/// One-time run: if the user deletes it from the CMS, it won't be recreated until a new DB.
/// </summary>
public static class BrixLandingSeeder
{
    public static void SeedIfEmpty(BrixDbContext db)
    {
        if (db.Pages.Any(p => p.Slug == "brixcms"))
            return;

        var page = new Page
        {
            Id = Guid.NewGuid(),
            Title = "BrixCMS.Open",
            Slug = "brixcms",
            IsPublished = true,
            PublishedAt = DateTime.UtcNow,
            PageType = "landing",
            JsonData = "{}"
        };

        db.Pages.Add(page);
        db.SaveChanges();
    }
}
