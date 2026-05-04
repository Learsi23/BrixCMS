using TestBrixCMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace TestBrixCMS.Controllers;

public class SitemapController : Controller
{
    private readonly BrixDbContext _db;
    public SitemapController(BrixDbContext db) => _db = db;

    [HttpGet("/sitemap.xml")]
    public async Task<IActionResult> Sitemap()
    {
        var pages = await _db.Pages
            .Where(p => p.IsPublished)
            .OrderBy(p => p.SortOrder)
            .ToListAsync();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var page in pages)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{baseUrl}/{page.Slug}</loc>");
            if (page.PublishedAt.HasValue)
                sb.AppendLine($"    <lastmod>{page.PublishedAt.Value:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("    <changefreq>weekly</changefreq>");
            sb.AppendLine("    <priority>0.8</priority>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }

    [HttpGet("/robots.txt")]
    public IActionResult Robots()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var content = $"User-agent: *\nAllow: /\nDisallow: /Manager/\nSitemap: {baseUrl}/sitemap.xml\n";
        return Content(content, "text/plain");
    }
}
