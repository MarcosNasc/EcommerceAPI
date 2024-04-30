using System.Security.Claims;

namespace Ecommerce.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal claimPrincipal)
    {
        if (claimPrincipal == null) throw new ArgumentException(nameof(claimPrincipal));
        var claim = claimPrincipal.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }

    public static string GetUserEmail(this ClaimsPrincipal claimPrincipal)
    {
        if (claimPrincipal == null) throw new ArgumentException(nameof(claimPrincipal));
        var claim = claimPrincipal.FindFirst(ClaimTypes.Email);
        return claim?.Value;
    }
}