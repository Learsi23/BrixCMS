using BrixCMS.Open.Data;
using Microsoft.EntityFrameworkCore;

namespace BrixCMS.Open.Extensions;

public static class PageUrlExtensions
{
    /// <summary>
    /// Builds the full nested URL path for a page by walking up the ParentId chain.
    /// </summary>
    public static string GetFullPath(this Page page, IDictionary<Guid, Page> allPagesById)
    {
        var segments = new List<string>();
        var current = page;
        var safety = 0;
        while (current != null && safety++ < 32)
        {
            if (!string.IsNullOrEmpty(current.Slug))
                segments.Insert(0, current.Slug.Trim('/'));
            if (current.ParentId is { } pid && allPagesById.TryGetValue(pid, out var parent))
                current = parent;
            else
                current = null;
        }
        return "/" + string.Join('/', segments);
    }

    /// <summary>
    /// Resolves a multi-segment URL path by walking the ParentId chain segment by segment.
    /// e.g., "/services/seo" -> finds page with slug "seo" where parent slug is "services".
    /// </summary>
    public static async Task<Page?> ResolveByPathAsync(this IQueryable<Page> pages, string slug)
    {
        var segments = slug.Split('/', StringSplitOptions.RemoveEmptyEntries);
        Page? current = null;
        Guid? parentId = null;

        for (var i = 0; i < segments.Length; i++)
        {
            var seg = segments[i].ToLower();
            current = await pages
                .FirstOrDefaultAsync(p => p.ParentId == parentId
                                          && p.Slug != null
                                          && p.Slug.ToLower() == seg);
            if (current == null) break;
            parentId = current.Id;
        }

        if (current == null && segments.Length == 1)
        {
            var seg = segments[0].ToLower();
            current = await pages
                .FirstOrDefaultAsync(p => p.Slug != null && p.Slug.ToLower() == seg);
        }

        return current;
    }
}
