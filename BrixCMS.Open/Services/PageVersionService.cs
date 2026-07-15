using System.Text.Json;
using BrixCMS.Open.Data;
using Microsoft.EntityFrameworkCore;

namespace BrixCMS.Open.Services;

/// <summary>
/// Full-page undo history. A snapshot captures a page's own fields plus every block (roots and
/// children, with their ORIGINAL ids preserved) as they were at one instant — so restoring can
/// remap parent/child links the same way <c>ManagerController.PublishPage</c> already does when
/// importing a freshly-edited block tree.
/// </summary>
public class PageVersionService
{
    /// <summary>Keep at most this many snapshots per page — older ones are pruned after each save.</summary>
    public const int MaxVersionsPerPage = 20;

    private readonly BrixDbContext _db;

    public PageVersionService(BrixDbContext db) => _db = db;

    private sealed record BlockSnapshot(string OriginalId, string? Type, string? JsonData, int SortOrder, string? ParentId);

    /// <summary>Snapshots the page's CURRENT (pre-change) state. Call this before overwriting a
    /// page's blocks/fields so the previous state becomes a restore point.</summary>
    public async Task SnapshotAsync(Guid pageId, string label, string? createdByEmail = null)
    {
        var page = await _db.Pages.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pageId);
        if (page is null) return;

        var blocks = await _db.Blocks.AsNoTracking().Where(b => b.PageId == pageId).ToListAsync();
        var blocksJson = JsonSerializer.Serialize(blocks
            .OrderBy(b => b.SortOrder)
            .Select(b => new BlockSnapshot(b.Id.ToString(), b.Type, b.JsonData, b.SortOrder, b.ParentId?.ToString())));

        _db.PageVersions.Add(new PageVersion
        {
            PageId          = pageId,
            Label           = label,
            CreatedByEmail  = createdByEmail,
            Title           = page.Title,
            Slug            = page.Slug,
            JsonData        = page.JsonData,
            MetaDescription = page.MetaDescription,
            OgImage         = page.OgImage,
            MetaKeywords    = page.MetaKeywords,
            BlocksJson      = blocksJson,
        });
        await _db.SaveChangesAsync();

        // Prune beyond the cap so history doesn't grow unbounded.
        var extra = await _db.PageVersions.Where(v => v.PageId == pageId)
            .OrderByDescending(v => v.CreatedAt)
            .Skip(MaxVersionsPerPage)
            .ToListAsync();
        if (extra.Count > 0)
        {
            _db.PageVersions.RemoveRange(extra);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<PageVersion>> ListAsync(Guid pageId) =>
        await _db.PageVersions.AsNoTracking()
            .Where(v => v.PageId == pageId)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();

    /// <summary>Restores a page to a prior snapshot. Snapshots the CURRENT state first (labeled
    /// "Before restore") so restoring is itself undoable.</summary>
    public async Task<bool> RestoreAsync(Guid pageId, Guid versionId, string? adminEmail = null)
    {
        var version = await _db.PageVersions.AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == versionId && v.PageId == pageId);
        var page = await _db.Pages.FindAsync(pageId);
        if (version is null || page is null) return false;

        await SnapshotAsync(pageId, "Before restore", adminEmail);

        page.Title           = version.Title;
        page.Slug            = version.Slug;
        page.JsonData        = version.JsonData;
        page.MetaDescription = version.MetaDescription;
        page.OgImage         = version.OgImage;
        page.MetaKeywords    = version.MetaKeywords;

        var existing = await _db.Blocks.Where(b => b.PageId == pageId).ToListAsync();
        _db.Blocks.RemoveRange(existing);
        await _db.SaveChangesAsync();

        var snapshots = JsonSerializer.Deserialize<List<BlockSnapshot>>(version.BlocksJson) ?? new();
        var idMap = new Dictionary<string, Guid>();

        foreach (var b in snapshots.Where(b => b.ParentId is null))
        {
            var newId = Guid.NewGuid();
            idMap[b.OriginalId] = newId;
            _db.Blocks.Add(new Block { Id = newId, PageId = pageId, Type = b.Type, JsonData = b.JsonData, SortOrder = b.SortOrder, ParentId = null });
        }
        foreach (var b in snapshots.Where(b => b.ParentId is not null))
        {
            var newId = Guid.NewGuid();
            var newParentId = b.ParentId is not null && idMap.TryGetValue(b.ParentId, out var pid) ? pid : (Guid?)null;
            _db.Blocks.Add(new Block { Id = newId, PageId = pageId, Type = b.Type, JsonData = b.JsonData, SortOrder = b.SortOrder, ParentId = newParentId });
        }
        await _db.SaveChangesAsync();
        return true;
    }
}
