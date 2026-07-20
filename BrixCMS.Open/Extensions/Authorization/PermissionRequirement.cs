using Microsoft.AspNetCore.Authorization;

namespace BrixCMS.Open.Extensions.Authorization;

/// <summary>
/// Requires the current admin to hold a specific named permission (one of the strings in
/// <see cref="AdminAuthorizationPolicies.PermissionCatalog"/>), unless their role bypasses
/// permission checks entirely — see <see cref="PermissionAuthorizationHandler"/>.
/// </summary>
public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission) => Permission = permission;
}
