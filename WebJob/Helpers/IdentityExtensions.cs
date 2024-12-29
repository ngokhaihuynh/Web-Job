using System.Security.Claims;
using System.Security.Principal;

namespace WebJob.Helpers
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this IPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                return userIdClaim?.Value;
            }
            return null;
        }
    }
}
