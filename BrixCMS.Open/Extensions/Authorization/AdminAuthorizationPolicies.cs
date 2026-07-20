using Microsoft.AspNetCore.Authorization;

namespace BrixCMS.Open.Extensions.Authorization;

/// <summary>
/// Registers the admin authorization handlers and named policies. Permission policy names are
/// "Permission:{name}" using the same lowercase strings stored in AdminUser.Permissions and
/// listed in Areas/Manager/Views/Admins/Index.cshtml — keep both in sync when adding a permission.
/// </summary>
public static class AdminAuthorizationPolicies
{
    public static readonly string[] PermissionCatalog =
    {
        "media", "configuration", "chatbot", "backup",
    };

    public const string OwnerOnly = "OwnerOnly";

    public static string Permission(string name) => $"Permission:{name}";

    public static IServiceCollection AddAdminAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, OwnerBypassHandler>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(OwnerOnly, p => p.Requirements.Add(new OwnerOnlyRequirement()));

            foreach (var permission in PermissionCatalog)
            {
                options.AddPolicy(Permission(permission),
                    p => p.Requirements.Add(new PermissionRequirement(permission)));
            }
        });

        return services;
    }
}
