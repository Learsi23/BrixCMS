using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BrixCMS.Open.Extensions.Authorization;

/// <summary>
/// Reproduces the semantics of the retired per-controller HasPermission() helpers: the "owner"
/// and "admin" roles bypass the permission list entirely (blanket access); everyone else
/// ("member") needs the matching "permission" claim, one of which is minted per entry in
/// AdminUser.Permissions at sign-in (see AdminAuthService.BuildPrincipal).
/// </summary>
public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
        if (role is "owner" or "admin")
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.HasClaim("permission", requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
