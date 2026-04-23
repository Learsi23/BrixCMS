using BrixCMS.Open.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BrixCMS.Open.Controllers;

public class CmsController : Controller
{
    private readonly BrixDbContext _db;
    public CmsController(BrixDbContext db) => _db = db;

    public async Task<IActionResult> Index(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            // ? First published page according to SortOrder
            var home = await _db.Pages
                .Where(p => p.IsPublished)
                .OrderBy(p => p.SortOrder)
                .FirstOrDefaultAsync();

            if (home == null)
                return Content("No published pages yet.");

            return Redirect("/" + home.Slug);
        }

        var page = await _db.Pages
            .FirstOrDefaultAsync(p => p.Slug.ToLower() == slug.ToLower());

        if (page == null) return Content($"Page '{slug}' not found.");
        if (!page.IsPublished) return Content($"The page '{slug}' exists but is not published.");

        page.Blocks = await _db.Blocks
            .Where(b => b.PageId == page.Id)
            .OrderBy(b => b.SortOrder)
            .ToListAsync();

        var ua = Request.Headers.UserAgent.ToString();
        var referrer = Request.Headers.Referer.ToString();
        _db.PageViews.Add(new PageView
        {
            Slug = slug ?? "/",
            UserAgent = ua.Length > 200 ? ua[..200] : ua,
            Referrer = referrer.Length > 500 ? referrer[..500] : referrer,
        });
        await _db.SaveChangesAsync();

        return View(page);
    }
}
