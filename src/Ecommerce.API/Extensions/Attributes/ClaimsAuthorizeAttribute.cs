using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.API.Extensions.Filters;

namespace Ecommerce.API.Extensions.Attributes;

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequirementClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, claimValue) };
    }
}