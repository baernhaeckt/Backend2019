using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Backend.Core.Features.UserManagement.Security;

namespace Backend.Core.Extensions
{
    public static class PrincipalExtensions
    {
        public static Guid Id(this IPrincipal principal) => Guid.Parse(principal.GetValueFromClaim(LeafClaimTypes.UserId));

        public static string Email(this IPrincipal principal) => principal.GetValueFromClaim(ClaimTypes.NameIdentifier);

        private static string GetValueFromClaim(this IPrincipal principal, string name)
        {
            var claimsIdentity = principal?.Identity as ClaimsIdentity;

            if (claimsIdentity == null)
            {
                return null;
            }

            return claimsIdentity.Claims.SingleOrDefault(c => string.Equals(c.Type, name, StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }
}