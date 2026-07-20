using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrixCMS.Open.Areas.Manager.Filters;

/// <summary>
/// Returns 401 Unauthorized (not a redirect) when the admin session is missing.
/// Use on API endpoints (POST/PUT/DELETE) that must not be publicly accessible.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireAdminApiAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            context.Result = new UnauthorizedObjectResult(new { error = "Admin authentication required." });
    }
}
