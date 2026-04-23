using BrixCMS.Open.Data;
using System.Text.Json;

namespace BrixCMS.Open.Services;

/// <summary>
/// Seeds a demo page showcasing available blocks.
/// </summary>
public static class BlockShowcaseSeeder
{
    public static void SeedIfEmpty(BrixDbContext db)
    {
        if (db.Pages.Any(p => p.Slug == "blocks-demo"))
            return;

        var page = new Page
        {
            Id = Guid.NewGuid(),
            Title = "Blocks Showcase",
            Slug = "blocks-demo",
            IsPublished = true,
            PublishedAt = DateTime.UtcNow,
            JsonData = "{}"
        };

        db.Pages.Add(page);
        db.SaveChanges();
    }
}
