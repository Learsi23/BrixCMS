using Microsoft.AspNetCore.Mvc;

namespace TestBrixCMS.Controllers;

public class LandingController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "BrixCMS — Din infrastruktur, dina regler.";
        return View();
    }
}
