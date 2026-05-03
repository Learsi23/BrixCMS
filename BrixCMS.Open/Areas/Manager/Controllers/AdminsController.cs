using BrixCMS.Open.Areas.Manager.Filters;
using BrixCMS.Open.Data;
using BrixCMS.Open.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrixCMS.Open.Areas.Manager.Controllers;

[Area("Manager")]
[RequireAdminLogin]
public class AdminsController : Controller
{
    private readonly AdminAuthService _auth;
    private readonly BrixDbContext _db;

    public AdminsController(AdminAuthService auth, BrixDbContext db)
    {
        _auth = auth;
        _db = db;
    }

    private bool CurrentUserIsOwner() =>
        HttpContext.Session.GetString("AdminIsOwner") == "1";

    // ── GET /Manager/Admins ───────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var currentEmail = HttpContext.Session.GetString("AdminEmail") ?? "";
        var currentUser  = await _auth.GetAdminByEmailAsync(currentEmail);

        ViewBag.CurrentEmail      = currentEmail;
        ViewBag.CurrentUser       = currentUser;
        ViewBag.TwoFactorEnabled  = currentUser?.TwoFactorEnabled ?? false;
        ViewBag.IsOwner           = CurrentUserIsOwner();

        var admins = await _auth.GetAllAdminsAsync();
        return View(admins);
    }

    // ── POST /Manager/Admins/Create ───────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string email, string name, string password)
    {
        if (!CurrentUserIsOwner())
            return RedirectToAction("Index", "Manager");

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            TempData["Error"] = "Email and password are required.";
            return RedirectToAction("Index");
        }

        if (await _db.AdminUsers.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
        {
            TempData["Error"] = $"An admin with email '{email}' already exists.";
            return RedirectToAction("Index");
        }

        await _auth.CreateAdminAsync(email, name, password);
        TempData["Success"] = $"Admin '{email}' created successfully.";
        return RedirectToAction("Index");
    }

    // ── POST /Manager/Admins/Delete ───────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!CurrentUserIsOwner())
            return RedirectToAction("Index", "Manager");

        var deleted = await _auth.DeleteAdminAsync(id);
        TempData[deleted ? "Success" : "Error"] = deleted
            ? "Admin deleted."
            : "Cannot delete the owner account.";

        return RedirectToAction("Index");
    }
}
