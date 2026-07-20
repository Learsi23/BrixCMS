using BrixCMS.Open.Data;
using BrixCMS.Open.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BrixCMS.Open.Areas.Manager.Controllers;

[Area("Manager")]
public class LoginController : Controller
{
    private readonly AdminAuthService _auth;

    public LoginController(AdminAuthService auth) => _auth = auth;

    private async Task SignInAdminAsync(AdminUser admin)
    {
        // Kept in Session purely for display purposes (nav bar, "edited by" trails) — the
        // security decisions (login-required, IsOwner, role, permissions) live in the cookie's
        // claims from here on, not in Session.
        HttpContext.Session.SetString("AdminEmail", admin.Email);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            AdminAuthService.BuildPrincipal(admin),
            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24) });
    }

    // ── GET /admin/manager ────────────────────────────────────────
    [HttpGet]
    public IActionResult Index(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
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
            // Partial auth: no cookie is issued yet, so a stolen/guessed password alone never
            // yields an authenticated HttpContext.User. Only a passed TOTP check below signs in.
            HttpContext.Session.SetString("AdminPreAuth", admin.Id.ToString());
            return RedirectToAction("TwoFactor", new { returnUrl });
        }

        // Full auth
        await SignInAdminAsync(admin);

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
        await SignInAdminAsync(admin);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Manager");
    }

    // ── GET /Manager/Login/Logout ─────────────────────────────────
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }
}
