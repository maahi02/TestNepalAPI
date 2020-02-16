using TestNepal.Dtos;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TestNepal.API.Filters
{
	/// <summary>
	/// 
	/// </summary>
	public class UserClaimAuthorizeAttribute : AuthorizeAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		public UserClaimAuthorizeAttribute()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="actionContext"></param>
		/// <returns></returns>
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var user = actionContext.RequestContext.Principal as ClaimsPrincipal;
			var hasUserIdClaim = user.Claims.Any(c => c.Type == Constants.API_CLAIM_TYPE_USER_ID);
			
			var isAuthorized =
                hasUserIdClaim && 
				base.IsAuthorized(actionContext);
            //Api.AutofacConfig.ConfigureContainer();
			return isAuthorized;
		}
	}
}