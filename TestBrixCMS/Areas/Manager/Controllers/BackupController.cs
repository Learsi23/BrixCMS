using TestBrixCMS.Areas.Manager.Filters;
using TestBrixCMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TestBrixCMS.Areas.Manager.Controllers;

[Area("Manager")]
[RequireAdminLogin]
public class BackupController : Controller
{
    private readonly BrixDbContext _db;
    public BackupController(BrixDbContext db) => _db = db;

    // GET /Manager/Backup — shows the backup/restore UI
    public IActionResult Index()
    {
        return View();
    }

    // GET /Manager/Backup/Export — downloads a JSON backup of all pages + blocks
    [HttpGet]
    public async Task<IActionResult> Export()
    {
        var pages = await _db.Pages.ToListAsync();
        var blocks = await _db.Blocks.ToListAsync();

        var backup = new
        {
            exportedAt = DateTime.UtcNow,
            version    = 1,
            pages      = pages.Select(p => new {
                p.Id, p.Title, p.Slug, p.JsonData, p.IsPublished,
                p.PublishedAt, p.SortOrder, p.PageType,
                p.MetaDescription, p.OgImage, p.MetaKeywords,
            }),
            blocks = blocks.Select(b => new {
                b.Id, b.Type, b.JsonData, b.SortOrder, b.PageId, b.ParentId,
            }),
        };

        var json = JsonSerializer.Serialize(backup, new JsonSerializerOptions { WriteIndented = true });
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        var fileName = $"brix-backup-{DateTime.UtcNow:yyyyMMdd-HHmmss}.json";
        return File(bytes, "application/json", fileName);
    }

    // POST /Manager/Backup/Import — restores pages + blocks from a JSON backup
    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file, bool replaceAll = false)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "No file selected.";
            return RedirectToAction(nameof(Index));
        }

        if (!file.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "Only .json backup files are accepted.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var json = await reader.ReadToEndAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("pages", out var pagesEl) ||
                !root.TryGetProperty("blocks", out var blocksEl))
            {
                TempData["Error"] = "Invalid backup file format.";
                return RedirectToAction(nameof(Index));
            }

            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var importedPages  = JsonSerializer.Deserialize<List<Page>>(pagesEl.GetRawText(), opts)  ?? new();
            var importedBlocks = JsonSerializer.Deserialize<List<Block>>(blocksEl.GetRawText(), opts) ?? new();

            if (replaceAll)
            {
                _db.Blocks.RemoveRange(_db.Blocks);
                _db.Pages.RemoveRange(_db.Pages);
                await _db.SaveChangesAsync();
            }

            // Skip pages/blocks that already exist (by Id) unless replaceAll
            var existingPageIds  = await _db.Pages.Select(p => p.Id).ToHashSetAsync();
            var existingBlockIds = await _db.Blocks.Select(b => b.Id).ToHashSetAsync();

            var newPages  = importedPages.Where(p => !existingPageIds.Contains(p.Id)).ToList();
            var newBlocks = importedBlocks.Where(b => !existingBlockIds.Contains(b.Id)).ToList();

            _db.Pages.AddRange(newPages);
            _db.Blocks.AddRange(newBlocks);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Imported {newPages.Count} pages and {newBlocks.Count} blocks.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Import failed: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
}
