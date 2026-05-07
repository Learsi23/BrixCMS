using BrixCMS.Open.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BrixCMS.Open.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private readonly BrixDbContext _db;
    private static readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

    public ApiController(BrixDbContext db)
    {
        _db = db;
    }

    // ── Legacy ────────────────────────────────────────────────────────────────

    [HttpGet("pages")]
    public async Task<IActionResult> GetPages()
    {
        var pages = await _db.Pages
            .Where(p => p.IsPublished)
            .OrderBy(p => p.SortOrder)
            .Select(p => new { p.Id, p.Title, p.Slug })
            .ToListAsync();

        return Ok(pages);
    }

    // ── Headless Content Delivery API ─────────────────────────────────────────

    /// <summary>List all published pages with full metadata.</summary>
    [HttpGet("content/pages")]
    public async Task<IActionResult> ContentPages()
    {
        var pages = await _db.Pages
            .Where(p => p.IsPublished)
            .OrderBy(p => p.SortOrder)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Slug,
                p.MetaDescription,
                p.OgImage,
                p.MetaKeywords,
                p.PublishedAt,
                p.SortOrder,
                p.PageType,
            })
            .ToListAsync();

        return Ok(pages);
    }

    /// <summary>Get a single published page with its block tree.</summary>
    [HttpGet("content/pages/{slug}")]
    public async Task<IActionResult> ContentPage(string slug)
    {
        var page = await _db.Pages
            .FirstOrDefaultAsync(p => p.IsPublished && p.Slug == slug);

        if (page is null)
            return NotFound(new { error = "Page not found." });

        var allBlocks = await _db.Blocks
            .Where(b => b.PageId == page.Id)
            .OrderBy(b => b.SortOrder)
            .ToListAsync();

        // Build nested block tree (top-level blocks contain children)
        var blockMap = allBlocks.ToDictionary(b => b.Id);
        var roots = new List<object>();

        foreach (var block in allBlocks)
        {
            if (block.ParentId is not null)
                continue; // handled when its parent is processed

            roots.Add(BuildBlockNode(block, allBlocks));
        }

        return Ok(new
        {
            page.Id,
            page.Title,
            page.Slug,
            page.MetaDescription,
            page.OgImage,
            page.MetaKeywords,
            page.PublishedAt,
            page.PageType,
            Blocks = roots,
        });
    }

    /// <summary>Flat list of raw blocks for a page — useful for custom renderers.</summary>
    [HttpGet("content/pages/{slug}/blocks")]
    public async Task<IActionResult> ContentBlocks(string slug)
    {
        var pageId = await _db.Pages
            .Where(p => p.IsPublished && p.Slug == slug)
            .Select(p => (Guid?)p.Id)
            .FirstOrDefaultAsync();

        if (pageId is null)
            return NotFound(new { error = "Page not found." });

        var rawBlocks = await _db.Blocks
            .Where(b => b.PageId == pageId)
            .OrderBy(b => b.SortOrder)
            .Select(b => new { b.Id, b.ParentId, b.Type, b.SortOrder, b.JsonData })
            .ToListAsync();

        var blocks = rawBlocks.Select(b =>
        {
            JsonElement? data = null;
            if (!string.IsNullOrEmpty(b.JsonData))
            {
                try { data = JsonSerializer.Deserialize<JsonElement>(b.JsonData); } catch { }
            }
            return new { b.Id, b.ParentId, b.Type, b.SortOrder, Data = data };
        }).ToList();

        return Ok(blocks);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static object BuildBlockNode(Block block, List<Block> all)
    {
        var children = all
            .Where(b => b.ParentId == block.Id)
            .OrderBy(b => b.SortOrder)
            .Select(child => BuildBlockNode(child, all))
            .ToList();

        JsonElement? data = null;
        if (!string.IsNullOrEmpty(block.JsonData))
        {
            try { data = JsonSerializer.Deserialize<JsonElement>(block.JsonData); }
            catch { /* malformed JSON — return null data */ }
        }

        return new
        {
            block.Id,
            block.ParentId,
            block.Type,
            block.SortOrder,
            Data = data,
            Children = children,
        };
    }

    // ── Newsletter ────────────────────────────────────────────────────────────

    public record SubscribeRequest([Required, EmailAddress] string Email, string? Name);

    [HttpPost("newsletter/subscribe")]
    public async Task<IActionResult> NewsletterSubscribe([FromBody] SubscribeRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { error = "Invalid email address." });

        var normalised = req.Email.Trim().ToLowerInvariant();

        var exists = await _db.Subscribers.AnyAsync(s => s.Email == normalised);
        if (exists)
            return Conflict(new { error = "You are already subscribed." });

        _db.Subscribers.Add(new Subscriber { Email = normalised, Name = req.Name?.Trim() });
        await _db.SaveChangesAsync();

        return Ok(new { ok = true });
    }
}
