using System;
using System.Linq;
using System.Security.Claims;

namespace Evant.Helpers
{
    public static class UserHelper
    {
        public static Guid GetUserId(this ClaimsPrincipal userClaims)
        {
            var claim = userClaims.Claims.FirstOrDefault(p => p.Type == "userId");
            return Guid.Parse(claim.Value);
        }

        public static void Logout(this ClaimsPrincipal userClaims)
        {
            var claim = userClaims;
            
        }

    }
}
