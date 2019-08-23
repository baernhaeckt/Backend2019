using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Backend.Extensions
{
    public static class PrincipalExtensions
    {
        public static string Id(this IPrincipal principal) => principal.GetValueFromClaim("Id");

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