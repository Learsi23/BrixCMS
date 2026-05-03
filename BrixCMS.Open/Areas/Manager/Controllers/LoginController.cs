using BrixCMS.Open.Data;
using BrixCMS.Open.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BrixCMS.Open.Areas.Manager.Controllers;

[Area("Manager")]
public class LoginController : Controller
{
    private readonly AdminAuthService _auth;

    public LoginController(AdminAuthService auth) => _auth = auth;

    // ── GET /admin/manager ────────────────────────────────────────
    [HttpGet]
    public IActionResult Index(string? returnUrl = null)
    {
        if (HttpContext.Session.GetString("AdminAuth") == "1")
            return RedirectToAction("Index", "Manager");

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    // ── POST /admin/manager ───────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Index(string email, string password, string? returnUrl = null)
    {
        var admin = await _auth.GetAdminByEmailAsync(email);

        if (admin == null ||
            !AdminAuthService.VerifyPassword(password, admin.PasswordHash, admin.PasswordSalt))
        {
            ViewBag.Error = "Incorrect email or password.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        if (admin.TwoFactorEnabled)
        {
            // Partial auth: store admin ID for the 2FA step
            HttpContext.Session.SetString("AdminPreAuth", admin.Id.ToString());
            return RedirectToAction("TwoFactor", new { returnUrl });
        }

        // Full auth
        HttpContext.Session.SetString("AdminAuth", "1");
        HttpContext.Session.SetString("AdminEmail", admin.Email);
        HttpContext.Session.SetString("AdminIsOwner", admin.IsOwner ? "1" : "0");

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Manager");
    }

    // ── GET /Manager/Login/TwoFactor ─────────────────────────────
    [HttpGet]
    public IActionResult TwoFactor(string? returnUrl = null)
    {
        if (HttpContext.Session.GetString("AdminPreAuth") == null)
            return RedirectToAction("Index");

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    // ── POST /Manager/Login/TwoFactor ────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TwoFactor(string code, string? returnUrl = null)
    {
        var preAuthId = HttpContext.Session.GetString("AdminPreAuth");
        if (preAuthId == null) return RedirectToAction("Index");

        AdminUser? admin = int.TryParse(preAuthId, out var adminId)
            ? await _auth.GetAdminByIdAsync(adminId)
            : await _auth.GetAdminAsync();

        if (admin == null || admin.TwoFactorSecret == null ||
            !AdminAuthService.VerifyTotp(admin.TwoFactorSecret, code))
        {
            ViewBag.Error = "Incorrect code. Please try again.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        HttpContext.Session.Remove("AdminPreAuth");
        HttpContext.Session.SetString("AdminAuth", "1");
        HttpContext.Session.SetString("AdminEmail", admin.Email);
        HttpContext.Session.SetString("AdminIsOwner", admin.IsOwner ? "1" : "0");

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Manager");
    }

    // ── GET /Manager/Login/Logout ─────────────────────────────────
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
