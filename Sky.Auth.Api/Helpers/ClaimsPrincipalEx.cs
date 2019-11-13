using System.Linq;
using System.Security.Claims;

namespace Sky.Auth.Api.Helpers
{
    public static class ClaimsPrincipalEx
    {
        private static string GetClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.Claims.FirstOrDefault(a => a.Type == claimType)?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return GetClaim(claimsPrincipal, ClaimTypes.Name);
        }

        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return GetClaim(claimsPrincipal, ClaimTypes.Email);
        }
    }
}
