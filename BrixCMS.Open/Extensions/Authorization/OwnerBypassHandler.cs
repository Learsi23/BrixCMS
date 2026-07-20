using Microsoft.AspNetCore.Authorization;

namespace BrixCMS.Open.Extensions.Authorization;

/// <summary>
/// The owner satisfies every authorization policy — team management, settings, all
/// permission-gated features — without needing a claim/handler per policy. Registered against
/// the base IAuthorizationRequirement so it runs for every requirement type added anywhere in
/// the app (AuthorizationHandler&lt;IAuthorizationRequirement&gt; matches all requirements via
/// context.Requirements.OfType&lt;IAuthorizationRequirement&gt;()).
/// </summary>
public sealed class OwnerBypassHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        if (context.User.HasClaim("AdminIsOwner", "1"))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
