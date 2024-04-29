namespace Ecommerce.API.Extensions;

public class CustomAuthorization
{
    public static bool ValidateUserClaims(HttpContext context,string claimName,string claimValue)
    {
         var result = context.User.Identity.IsAuthenticated &&
               context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
         return result;
    }
}