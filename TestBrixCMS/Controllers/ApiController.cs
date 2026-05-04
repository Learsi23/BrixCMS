using TestBrixCMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TestBrixCMS.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private readonly BrixDbContext _db;

    public ApiController(BrixDbContext db)
    {
        _db = db;
    }

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
