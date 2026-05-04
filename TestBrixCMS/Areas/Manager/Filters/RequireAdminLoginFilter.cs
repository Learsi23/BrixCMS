using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestBrixCMS.Areas.Manager.Filters;

/// <summary>
/// Redirects to the login page if the admin session is not active.
/// Apply to any Manager-area controller or action that requires authentication.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireAdminLoginAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;

        // Full auth
        if (session.GetString("AdminAuth") == "1")
            return;

        // Redirect to login, preserving the return URL
        var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
        context.Result = new RedirectToActionResult(
            actionName:     "Index",
            controllerName: "Login",
            routeValues:    new { area = "Manager", returnUrl });
    }
}
