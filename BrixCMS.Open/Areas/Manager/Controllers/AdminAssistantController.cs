using BrixCMS.Open.Areas.Manager.Filters;
using BrixCMS.Open.Data;
using BrixCMS.Open.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrixCMS.Open.Areas.Manager.Controllers;

[Area("Manager")]
[RequireAdminLogin]
public class AdminAssistantController : Controller
{
    private const string ConfigKey = "admin-chat-default-image";

    private readonly BrixDbContext _db;
    private readonly PromptsService _prompts;

    public AdminAssistantController(BrixDbContext db, PromptsService prompts)
    {
        _db      = db;
        _prompts = prompts;
    }

    public IActionResult Index()
    {
        var defaultImage = _db.SiteConfig
            .FirstOrDefault(c => c.Key == ConfigKey)?.JsonData?.Trim('"');

        ViewBag.DefaultImage  = defaultImage ?? "";
        ViewBag.SystemPrompt  = _prompts.GenerateAdminChatSystemPrompt();
        ViewBag.Success       = TempData["Success"] as string;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SaveSettings(string defaultImage)
    {
        var value = defaultImage?.Trim() ?? "";
        var cfg   = _db.SiteConfig.FirstOrDefault(c => c.Key == ConfigKey);

        if (cfg != null)
            cfg.JsonData = $"\"{value}\"";
        else
            _db.SiteConfig.Add(new SiteConfig { Id = 0, Key = ConfigKey, JsonData = $"\"{value}\"" });

        await _db.SaveChangesAsync();
        TempData["Success"] = "Settings saved.";
        return RedirectToAction(nameof(Index));
    }
}
