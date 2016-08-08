using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using PROYECT.Dominio.Helpers;

namespace PROYECT.WebAPI.Extensions
{
	public static class UserExtensions
	{
		public static bool IsAdmin(this IPrincipal user)
		{
			return ((ClaimsPrincipal)user).HasClaim(ClaimTypes.Role, AppConfigHelper.ADAdminGroup);
		}

		public static bool IsAdmin(this IIdentity user)
		{
			return ((ClaimsIdentity)user).HasClaim(ClaimTypes.Role, AppConfigHelper.ADAdminGroup);
		}

        public static bool IsEnte(this IPrincipal user)
        {
            return ((ClaimsPrincipal)user).HasClaim(ClaimTypes.Role, AppConfigHelper.ADEnteGroup);
        }

        public static bool IsEnte(this IIdentity user)
        {
            return ((ClaimsIdentity)user).HasClaim(ClaimTypes.Role, AppConfigHelper.ADEnteGroup);
        }

        public static Int64 GetJurisdiccionId(this IPrincipal user)
		{
            if (user.IsAdmin())
            {
                return -1;
            }
            return Int64.Parse(((ClaimsPrincipal)user).Claims.Single(c => c.Type == "jurisdiccion").Value);
		}

		public static Int64 GetJurisdiccionId(this IIdentity user)
		{
            if (user.IsAdmin())
            {
                return -1;
            }
            return Int64.Parse(((ClaimsIdentity)user).Claims.Single(c => c.Type == "jurisdiccion").Value);
		}
	}
}
