using Microsoft.AspNetCore.Authorization;

namespace BrixCMS.Open.Extensions.Authorization;

/// <summary>
/// Marker requirement for owner-only actions (team management). Has no dedicated handler — it
/// is only ever satisfied by <see cref="OwnerBypassHandler"/>, which succeeds any requirement
/// for the tenant owner. A non-owner never satisfies it.
/// </summary>
public sealed class OwnerOnlyRequirement : IAuthorizationRequirement
{
}
