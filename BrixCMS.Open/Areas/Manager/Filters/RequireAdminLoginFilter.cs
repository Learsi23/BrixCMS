using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrixCMS.Open.Areas.Manager.Filters;

/// <summary>
/// Redirects to the login page if the admin session is not active.
/// Apply to any Manager-area controller or action that requires authentication.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireAdminLoginAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Full auth — backed by the standard cookie-auth pipeline (UseAuthentication in
        // Program.cs), not a session flag.
        if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            return;

        // Redirect to login, preserving the return URL
        var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
        context.Result = new RedirectToActionResult(
            actionName:     "Index",
            controllerName: "Login",
            routeValues:    new { area = "Manager", returnUrl });
    }
}
