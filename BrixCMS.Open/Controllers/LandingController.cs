using Microsoft.AspNetCore.Mvc;

namespace BrixCMS.Open.Controllers;

public class LandingController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "BrixCMS — Din infrastruktur, dina regler.";
        return View();
    }
}
