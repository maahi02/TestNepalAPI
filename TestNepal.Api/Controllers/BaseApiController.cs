using TestNepal.Api.Helpers;
using TestNepal.Dtos;
using System;
using System.Security.Claims;
using System.Web.Http;

namespace TestNepal.Api.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseApiController : ApiController
	{
		/// <summary>
		/// 
		/// </summary>
		protected ClaimsPrincipal CurrentPrincipal
		{
			get
			{
				return User as ClaimsPrincipal;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected string GetUserName()
        {
            return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, ClaimTypes.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected Guid GetUserId()
        {
            return AuthHelper.GetClaim<Guid>(CurrentPrincipal.Claims, Constants.API_CLAIM_TYPE_USER_ID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NonAction]
		protected string GetFirstName()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.TestNepal_FIRSTNAME);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[NonAction]
		protected string GetLastName()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.TestNepal_LASTNAME);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[NonAction]
		protected string GetEmail()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.TestNepal_EMAIL);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[NonAction]
		protected string GetPhone()
		{
			return AuthHelper.GetClaim<string>(CurrentPrincipal.Claims, Constants.TestNepal_PHONE);
		}
	}
}