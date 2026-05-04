using BrixCMS.Open.Areas.Manager.Filters;
using BrixCMS.Open.Data;
using BrixCMS.Open.Models;
using BrixCMS.Open.Services;
using BrixCMS.Open.Services.Ingestion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BrixCMS.Open.Areas.Manager.Controllers;

[Area("Manager")]
[RequireAdminLogin]
[AutoValidateAntiforgeryToken]
public class ConfigurationController : Controller
{
    private readonly BrixDbContext _db;
    private readonly DataIngestor _ingestor;
    private readonly IWebHostEnvironment _env;
    private readonly SemanticSearch _search;
    private readonly AdminAuthService _auth;
    private readonly ApiKeyService _apiKeys;
    private readonly ILogger<ConfigurationController> _logger;

    public ConfigurationController(
        BrixDbContext db,
        DataIngestor ingestor,
        IWebHostEnvironment env,
        SemanticSearch search,
        AdminAuthService auth,
        ApiKeyService apiKeys,
        ILogger<ConfigurationController> logger)
    {
        _db       = db;
        _ingestor = ingestor;
        _env      = env;
        _search   = search;
        _auth     = auth;
        _apiKeys  = apiKeys;
        _logger   = logger;
    }

    // ── Navbar & Footer ───────────────────────────────────────────────────────

    public IActionResult Index()
    {
        var siteConfig = _db.SiteConfig.FirstOrDefault(c => c.Key == "site");
        SiteSettings siteSettings = new();

        if (siteConfig != null && !string.IsNullOrEmpty(siteConfig.JsonData))
        {
            try
            {
                siteSettings = JsonSerializer.Deserialize<SiteSettings>(siteConfig.JsonData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            catch { }
        }

        return View(new ConfiguracionViewModel { SiteSettings = siteSettings });
    }

    [HttpPost]
    public IActionResult SaveSiteSettings(SiteSettingsViewModel model)
    {
        var siteConfig = _db.SiteConfig.FirstOrDefault(c => c.Key == "site");
        SiteSettings siteSettings = new();

        if (siteConfig != null && !string.IsNullOrEmpty(siteConfig.JsonData))
        {
            try
            {
                siteSettings = JsonSerializer.Deserialize<SiteSettings>(siteConfig.JsonData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            catch { }
        }

        siteSettings.Navbar = new NavbarSettings
        {
            BackgroundColor = model.NavbarBackgroundColor ?? "#ffffff",
            TextColor       = model.NavbarTextColor       ?? "#000000",
            Logo            = model.NavbarLogo            ?? "",
            LogoAltText     = model.NavbarLogoAltText     ?? "Logo",
            LogoWidth       = model.NavbarLogoWidth       ?? "150px",
            LogoLink        = model.NavbarLogoLink        ?? "/",
            IsSticky        = model.NavbarIsSticky,
            HasShadow       = model.NavbarHasShadow,
            PaddingVertical = model.NavbarPaddingVertical ?? "py-3",
            MenuItems       = model.MenuItems             ?? new(),
        };

        siteSettings.Footer = new FooterSettings
        {
            BackgroundColor       = model.FooterBackgroundColor       ?? "#1a1a1a",
            TextColor             = model.FooterTextColor             ?? "#ffffff",
            Logo                  = model.FooterLogo                  ?? "",
            LogoAltText           = model.FooterLogoAltText           ?? "Logo",
            LogoWidth             = model.FooterLogoWidth             ?? "150px",
            LogoPosition          = model.FooterLogoPosition          ?? "left",
            ShowPagesColumn       = model.FooterShowPagesColumn,
            PagesColumnTitle      = model.FooterPagesColumnTitle      ?? "Pages",
            Pages                 = model.FooterPages                 ?? new(),
            ShowSocialMediaColumn = model.FooterShowSocialMediaColumn,
            SocialMediaColumnTitle = model.FooterSocialMediaColumnTitle ?? "Follow us",
            SocialMedia           = model.SocialMedia                 ?? new(),
            ShowCopyrightRow      = model.FooterShowCopyrightRow,
            CompanyName           = model.CompanyName                 ?? "",
            CompanyNumber         = model.CompanyNumber               ?? "",
            CopyrightText         = model.CopyrightText               ?? "All rights reserved",
            ShowHorizontalLine    = model.FooterShowHorizontalLine,
            PaddingVertical       = model.FooterPaddingVertical       ?? "py-6",
            ColumnsGap            = model.FooterColumnsGap            ?? "gap-8",
        };

        if (siteConfig == null)
        {
            siteConfig = new SiteConfig { Key = "site", JsonData = "{}" };
            _db.SiteConfig.Add(siteConfig);
        }
        siteConfig.JsonData = JsonSerializer.Serialize(siteSettings,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _db.SaveChanges();
        TempData["Success"] = "Settings saved";
        return RedirectToAction(nameof(Index));
    }

    // ── Chatbot (PDFs + Ollama + Account security) ────────────────────────────

    public async Task<IActionResult> Chatbot()
    {
        var dataPath = Path.Combine(_env.WebRootPath, "Data");
        if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);

        var pdfFiles = Directory.GetFiles(dataPath, "*.pdf")
            .Select(f => new FileInfo(f))
            .OrderByDescending(f => f.LastWriteTime)
            .ToList();

        var activeConfig    = _apiKeys.GetActiveProviderConfig();

        ViewBag.OllamaUrl        = _apiKeys.GetOllamaUrl();
        ViewBag.OllamaModel      = activeConfig?.Provider == "ollama" ? activeConfig.Model : "";
        ViewBag.PdfFiles         = pdfFiles.Select(f => new PdfFileInfo
        {
            Name         = f.Name,
            Size         = f.Length,
            LastModified = f.LastWriteTime,
            FullPath     = f.FullName,
        }).ToList();

        return View();
    }

    // ── Ollama ────────────────────────────────────────────────────────────────

    [HttpPost]
    public async Task<IActionResult> SaveOllamaConfig(string url, string model)
    {
        url   = string.IsNullOrWhiteSpace(url)   ? "http://localhost:11434" : url.Trim();
        model = string.IsNullOrWhiteSpace(model) ? "llama3.2:3b" : model.Trim();

        await _apiKeys.SaveOllamaUrlAsync(url);
        await _apiKeys.SetActiveProviderAsync("ollama", model);
        TempData["ChatbotSuccess"] = $"Ollama configured: {model}";
        return RedirectToAction(nameof(Chatbot));
    }

    [HttpGet]
    public async Task<IActionResult> GetOllamaModels()
    {
        try
        {
            var url = _apiKeys.GetOllamaUrl();
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            var response = await client.GetAsync($"{url}/api/tags");
            if (!response.IsSuccessStatusCode)
                return Json(new { ok = false, error = "Ollama not reachable" });

            var json = await response.Content.ReadAsStringAsync();
            var doc  = JsonDocument.Parse(json);
            var models = new List<string>();

            if (doc.RootElement.TryGetProperty("models", out var modelsArray))
            {
                foreach (var m in modelsArray.EnumerateArray())
                    if (m.TryGetProperty("name", out var nameProp))
                        models.Add(nameProp.GetString() ?? "");
            }

            return Json(new { ok = true, models });
        }
        catch
        {
            return Json(new { ok = false, error = "Cannot connect to Ollama" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> PullOllamaModel([FromBody] JsonElement body)
    {
        var modelName = body.GetProperty("model").GetString()?.Trim();
        if (string.IsNullOrWhiteSpace(modelName))
            return Json(new { ok = false, error = "Model name required" });

        try
        {
            var url = _apiKeys.GetOllamaUrl();
            using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(30) };
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(new { name = modelName, stream = false }),
                System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{url}/api/pull", content);
            if (!response.IsSuccessStatusCode)
            {
                var errBody = await response.Content.ReadAsStringAsync();
                return Json(new { ok = false, error = errBody });
            }

            return Json(new { ok = true, model = modelName });
        }
        catch (Exception ex)
        {
            return Json(new { ok = false, error = ex.Message });
        }
    }

    // ── PDFs ──────────────────────────────────────────────────────────────────

    [HttpPost]
    public async Task<IActionResult> UploadPdf(IFormFile file)
    {
        if (file == null || file.Length == 0 || !file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ChatbotError"] = "Please select a valid PDF file.";
            return RedirectToAction(nameof(Chatbot));
        }

        var dataPath = Path.Combine(_env.WebRootPath, "Data");
        Directory.CreateDirectory(dataPath);
        var filePath = Path.Combine(dataPath, Path.GetFileName(file.FileName));

        await using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        try
        {
            await _ingestor.IngestPdfAsync(filePath);
            TempData["ChatbotSuccess"] = $"'{file.FileName}' uploaded and indexed.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ingesting PDF {File}", file.FileName);
            TempData["ChatbotError"] = $"Uploaded but indexing failed: {ex.Message}";
        }

        return RedirectToAction(nameof(Chatbot));
    }

    [HttpPost]
    public async Task<IActionResult> DeletePdf(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return RedirectToAction(nameof(Chatbot));

        var filePath = Path.Combine(_env.WebRootPath, "Data", fileName);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
            await _search.DeleteDocumentAsync(fileName);
            TempData["ChatbotSuccess"] = $"'{fileName}' deleted.";
        }

        return RedirectToAction(nameof(Chatbot));
    }

    [HttpPost]
    public async Task<IActionResult> ReingestAll()
    {
        var dataPath = Path.Combine(_env.WebRootPath, "Data");
        var pdfs     = Directory.Exists(dataPath) ? Directory.GetFiles(dataPath, "*.pdf") : Array.Empty<string>();
        int count    = 0;

        foreach (var file in pdfs)
        {
            try { await _ingestor.IngestPdfAsync(file); count++; }
            catch (Exception ex) { _logger.LogError(ex, "Reingest failed: {File}", file); }
        }

        TempData["ChatbotSuccess"] = $"Re-indexed {count} PDF(s).";
        return RedirectToAction(nameof(Chatbot));
    }

    // ── Account security ──────────────────────────────────────────────────────

    [HttpPost]
    public async Task<IActionResult> ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
        {
            TempData["AccountError"] = "Email cannot be empty.";
            return RedirectToAction("Index", "Admins");
        }
        var currentEmail = HttpContext.Session.GetString("AdminEmail") ?? "";
        var admin = await _auth.GetAdminByEmailAsync(currentEmail);
        if (admin != null)
        {
            await _auth.UpdateEmailAsync(admin.Id, newEmail.Trim());
            HttpContext.Session.SetString("AdminEmail", newEmail.Trim());
        }
        TempData["AccountSuccess"] = "Email updated.";
        return RedirectToAction("Index", "Admins");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var currentEmail = HttpContext.Session.GetString("AdminEmail") ?? "";
        var admin = await _auth.GetAdminByEmailAsync(currentEmail);
        if (admin == null) return RedirectToAction("Index", "Admins");

        if (!AdminAuthService.VerifyPassword(currentPassword, admin.PasswordHash, admin.PasswordSalt))
        {
            TempData["AccountError"] = "Current password is incorrect.";
            return RedirectToAction("Index", "Admins");
        }
        if (newPassword != confirmPassword)
        {
            TempData["AccountError"] = "New passwords do not match.";
            return RedirectToAction("Index", "Admins");
        }
        if (newPassword.Length < 8)
        {
            TempData["AccountError"] = "Password must be at least 8 characters.";
            return RedirectToAction("Index", "Admins");
        }

        await _auth.UpdatePasswordAsync(admin.Id, newPassword);
        TempData["AccountSuccess"] = "Password updated.";
        return RedirectToAction("Index", "Admins");
    }

    [HttpPost]
    public IActionResult GenerateTotpSetup()
    {
        var secret = AdminAuthService.GenerateTotpSecret();
        HttpContext.Session.SetString("PendingTotpSecret", secret);
        var email  = HttpContext.Session.GetString("AdminEmail") ?? "admin";
        var otpUrl = AdminAuthService.GetOtpAuthUrl(secret, email);
        return Json(new
        {
            secret,
            otpUrl,
            qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={Uri.EscapeDataString(otpUrl)}",
        });
    }

    [HttpPost]
    public async Task<IActionResult> EnableTwoFactor(string totpCode)
    {
        var currentEmail  = HttpContext.Session.GetString("AdminEmail") ?? "";
        var admin         = await _auth.GetAdminByEmailAsync(currentEmail);
        var pendingSecret = HttpContext.Session.GetString("PendingTotpSecret");

        if (admin == null || string.IsNullOrEmpty(pendingSecret))
        {
            TempData["AccountError"] = "Session expired. Restart 2FA setup.";
            return RedirectToAction("Index", "Admins");
        }
        if (!AdminAuthService.VerifyTotp(pendingSecret, totpCode))
        {
            TempData["AccountError"] = "Incorrect code. Try again.";
            return RedirectToAction("Index", "Admins");
        }

        await _auth.SetTwoFactorAsync(admin.Id, true, pendingSecret);
        HttpContext.Session.Remove("PendingTotpSecret");
        TempData["AccountSuccess"] = "Two-factor authentication enabled.";
        return RedirectToAction("Index", "Admins");
    }

    [HttpPost]
    public async Task<IActionResult> DisableTwoFactor(string password)
    {
        var currentEmail = HttpContext.Session.GetString("AdminEmail") ?? "";
        var admin = await _auth.GetAdminByEmailAsync(currentEmail);
        if (admin == null) return RedirectToAction("Index", "Admins");

        if (!AdminAuthService.VerifyPassword(password, admin.PasswordHash, admin.PasswordSalt))
        {
            TempData["AccountError"] = "Incorrect password.";
            return RedirectToAction("Index", "Admins");
        }

        await _auth.SetTwoFactorAsync(admin.Id, false, null);
        TempData["AccountSuccess"] = "Two-factor authentication disabled.";
        return RedirectToAction("Index", "Admins");
    }

    // ── Analytics (AJAX) ─────────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> AnalyticsStats([FromQuery] string? period = "week")
    {
        var since = period switch
        {
            "today" => DateTime.UtcNow.Date,
            "month" => new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
            "all"   => DateTime.MinValue,
            _       => DateTime.UtcNow.AddDays(-7),
        };
        var views    = await _db.PageViews.Where(v => v.ViewedAt >= since).ToListAsync();
        var topPages = views
            .GroupBy(v => v.Slug)
            .Select(g => new { slug = g.Key, count = g.Count() })
            .OrderByDescending(x => x.count)
            .Take(10)
            .ToList();
        return Json(new { totalViews = views.Count, topPages });
    }
}

// ── ViewModels ────────────────────────────────────────────────────────────────

public class ConfiguracionViewModel
{
    public SiteSettings SiteSettings { get; set; } = new();
}

public class PdfFileInfo
{
    public string Name { get; set; } = "";
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string FullPath { get; set; } = "";

    public string FormattedSize => Size switch
    {
        < 1024           => $"{Size} B",
        < 1024 * 1024    => $"{Size / 1024.0:F1} KB",
        _                => $"{Size / (1024.0 * 1024.0):F1} MB",
    };
}

public class SiteSettingsViewModel
{
    public string? NavbarBackgroundColor { get; set; }
    public string? NavbarTextColor { get; set; }
    public string? NavbarLogo { get; set; }
    public string? NavbarLogoAltText { get; set; }
    public string? NavbarLogoWidth { get; set; }
    public string? NavbarLogoLink { get; set; }
    public bool NavbarIsSticky { get; set; } = true;
    public bool NavbarHasShadow { get; set; } = true;
    public string? NavbarPaddingVertical { get; set; }
    public List<MenuItemConfig>? MenuItems { get; set; }

    public string? FooterBackgroundColor { get; set; }
    public string? FooterTextColor { get; set; }
    public string? FooterLogo { get; set; }
    public string? FooterLogoAltText { get; set; }
    public string? FooterLogoWidth { get; set; }
    public string? FooterLogoPosition { get; set; }
    public bool FooterShowPagesColumn { get; set; }
    public string? FooterPagesColumnTitle { get; set; }
    public List<MenuItemConfig>? FooterPages { get; set; }
    public bool FooterShowSocialMediaColumn { get; set; }
    public string? FooterSocialMediaColumnTitle { get; set; }
    public List<SocialMediaConfig>? SocialMedia { get; set; }
    public bool FooterShowCopyrightRow { get; set; } = true;
    public string? CompanyName { get; set; }
    public string? CompanyNumber { get; set; }
    public string? CopyrightText { get; set; }
    public bool FooterShowHorizontalLine { get; set; } = true;
    public string? FooterPaddingVertical { get; set; }
    public string? FooterColumnsGap { get; set; }
}
