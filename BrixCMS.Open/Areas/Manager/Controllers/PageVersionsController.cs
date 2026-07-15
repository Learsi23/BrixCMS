using BrixCMS.Open.Areas.Manager.Filters;
using BrixCMS.Open.Data;
using BrixCMS.Open.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrixCMS.Open.Areas.Manager.Controllers;

/// <summary>Full-page undo history — lists snapshots taken automatically on every publish
/// (<see cref="PageVersionService"/>) and lets the admin roll a page back to one of them.</summary>
[Area("Manager")]
[RequireAdminLogin]
public class PageVersionsController : Controller
{
    private readonly BrixDbContext _db;
    private readonly PageVersionService _versions;

    public PageVersionsController(BrixDbContext db, PageVersionService versions)
    {
        _db = db;
        _versions = versions;
    }

    public async Task<IActionResult> Index(Guid pageId)
    {
        var page = await _db.Pages.FindAsync(pageId);
        if (page is null) return NotFound();

        ViewBag.Page = page;
        ViewBag.Success = TempData["Success"] as string;
        ViewBag.Error = TempData["Error"] as string;
        var versions = await _versions.ListAsync(pageId);
        return View(versions);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(Guid pageId, Guid versionId)
    {
        var ok = await _versions.RestoreAsync(pageId, versionId, HttpContext.Session.GetString("AdminEmail"));
        TempData["Success"] = ok ? "Page restored. The version right before this restore was saved too, in case you want to undo it." : null;
        if (!ok) TempData["Error"] = "Could not restore — the version or page was not found.";
        return RedirectToAction(nameof(Index), new { pageId });
    }
}
