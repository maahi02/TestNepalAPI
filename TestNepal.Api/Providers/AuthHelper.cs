using TestNepal.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace TestNepal.Api.Helpers
{
	public static class AuthHelper
	{
        internal static Guid GetCurrentTenantId()
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;
            if (user != null && user.Claims.Count() > 0)
            {
                return AuthHelper.GetClaim<Guid>(user.Claims, Constants.API_CLAIM_TYPE_TENANT_ID);
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[Constants.API_CLAIM_TYPE_TENANT_ID]))
                return new Guid(ConfigurationManager.AppSettings[Constants.API_CLAIM_TYPE_TENANT_ID]);
            return Guid.Empty;
        }

        internal static Guid GetCurrentUserId()
        {
            var user = System.Threading.Thread.CurrentPrincipal as ClaimsPrincipal;
            if (user != null)
            {
                return AuthHelper.GetClaim<Guid>(user.Claims, Constants.API_CLAIM_TYPE_USER_ID);
            }
            return Guid.Empty;
        }

        public static bool TryGetClaim(IEnumerable<Claim> claims, string claimType, out string claimValue)
		{
			claimValue = string.Empty;

			var foundClaim = claims.FirstOrDefault(x => x.Type.Equals(claimType, StringComparison.InvariantCultureIgnoreCase));
			if (foundClaim != null)
			{
				claimValue = foundClaim.Value;
				return true;
			}
			return false;
		}

		public static T GetClaim<T>(IEnumerable<Claim> claims, string claimType)
		{
			string claimValue;
			if (TryGetClaim(claims, claimType, out claimValue))
			{
				if (typeof(T) == typeof(Guid))
				{
					Guid claimValueHolder = Guid.Empty;
					Guid.TryParse(claimValue, out claimValueHolder);
					return (T)Convert.ChangeType(claimValueHolder, typeof(T));
				}
				return (T)Convert.ChangeType(claimValue, typeof(T));
			}
			return default(T);
		}
	}
}
