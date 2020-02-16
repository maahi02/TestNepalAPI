using TestNepal.Dtos;
using TestNepal.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TestNepal.Api.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TestNepal.Api.Controllers
{
    [RoutePrefix("api/User")]
    public class TSUserController : ApiController
    {
        private IUserService _userService;
     
        private IProfileService _profileService;
        public TSUserController(IUserService userService, IProfileService profileService)
        {
            _userService = userService;
            _profileService = profileService;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRegistration"></param>
        /// <returns></returns>
        /// 
        [HttpPost, Route("register")]
        public async Task<object> RegisterUser(UserRegistration userRegistration)
        {
            userRegistration.TenantId = AuthHelper.GetCurrentTenantId();

            IdentityResult identityResult = await _userService.CreateAsync(userRegistration);
            if (identityResult != null && identityResult.Succeeded)
            {
                string emailconfrimationCode = await _userService.GetEmailConfirmationCode(userRegistration.Id);
                var callbackUrl = System.Configuration.ConfigurationManager.AppSettings["WebappBaseUrl"] + "confirmemail?userId=" + userRegistration.Id + "&code=" + emailconfrimationCode;
                List<KeyNamePair> replacelist = new List<KeyNamePair>();
                replacelist.Add(new KeyNamePair() { Name = "[Link]", Value = callbackUrl });
                replacelist.Add(new KeyNamePair() { Name = "[Email Address]", Value = userRegistration.Email });
                
               // _emailService.Send(EmailTemplateType.RegistrationEmail.ToString(), replacelist, userRegistration.Email);
                return Common.JsonOkObject(
                        new { msg = "User registered successully. Please check email for instructions." }
                    );
            }
            else
            {
                return Common.JsonErrorObject(string.Join(Environment.NewLine, identityResult.Errors));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost, Route("confirmEmail")]
        public async Task<object> ConfirmEmail(string userId, string code)
        {
            if(string.IsNullOrEmpty(code) == false)
            {
                code = code.Replace(" ", "+");
            }
            IdentityResult identityResult = await _userService.ConfirmEmail(userId, code);
            if (identityResult != null && identityResult.Succeeded)
            {
                string email = _userService.GetEmailById(userId);
                if (!string.IsNullOrEmpty(email))
                {
                    //_emailService.Send(EmailTemplateType.RegistrationConfirmation.ToString(), null, email);
                }
                return Common.JsonOkObject(
                        new { msg = "Email Confirmed Successully" }
                    );
            }
            else
            {
                return Common.JsonErrorObject(identityResult == null ? "Not valid user or code." : string.Join(Environment.NewLine, identityResult.Errors));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, Route("forgotPassword")]
        public async Task<object> ForgotPassword(string userName)
        {
            IdentityUser identityUser = await _userService.FindUser(userName);
            if (identityUser != null)
            {
                Profile profile = _profileService.GetProfile(userName);
                string resetPwdCode = await _userService.GetResetPasswordCode(identityUser.Id);
                var callbackUrl = System.Configuration.ConfigurationManager.AppSettings["WebappBaseUrl"] + "resetpassword?userId=" + identityUser.Id + "&code=" + resetPwdCode;
                List<KeyNamePair> replacelist = new List<KeyNamePair>();
                replacelist.Add(new KeyNamePair() { Name = "[Link]", Value = callbackUrl });
                replacelist.Add(new KeyNamePair() { Name = "[Email Address]", Value = identityUser.Email });
                replacelist.Add(new KeyNamePair() { Name = "*|CUSTOMER_FIRSTNAME|*", Value = profile.FirstName });
                replacelist.Add(new KeyNamePair() { Name = "*|CUSTOMER_LASTNAME|*", Value = profile.LastName });
                //_emailService.Send(EmailTemplateType.ResetPassword.ToString(), replacelist, identityUser.Email);
                return Common.JsonOkObject(
                        new { msg = "Check email and follow to reset password." }
                    );
            }
            else
            {
                return Common.JsonErrorObject("Wrong username.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost, Route("forgotPasswordConfirm")]
        public async Task<object> forgotPasswordConfirm(string userId, string code, string newPassword)
        {
            if (string.IsNullOrEmpty(code) == false)
            {
                code = code.Replace(" ", "+");
            }
            IdentityResult identityResult = await _userService.ResetPassword(userId, code, newPassword);
            if (identityResult != null && identityResult.Succeeded)
            {
                string email = _userService.GetEmailById(userId);
                if (!string.IsNullOrEmpty(email))
                {
                   // _emailService.Send(EmailTemplateType.PasswordResetconfirmation.ToString(), null, email);
                }
                return Common.JsonOkObject(
                    new { msg = "Password changed successfully." }
                );
            }
            else
            {
                return Common.JsonErrorObject(identityResult == null ? "Not valid user or code." : string.Join(Environment.NewLine, identityResult.Errors));
            }
        }
    }
}
