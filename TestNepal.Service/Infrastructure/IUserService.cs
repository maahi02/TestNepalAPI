using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNepal.Entities;
using System.Linq.Expressions;
using TestNepal.Dtos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TestNepal.Service.Infrastructure
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(UserRegistration user);
        Task<IdentityUser> FindUser(string userName);
        string GetEmailById(string Id);

        IEnumerable<ApplicationUser> GetAll(Expression<Func<ApplicationUser, bool>> where = null, params Expression<Func<ApplicationUser, object>>[] includeExpressions);
        void Login();
        string GetUserId(string UserName);
        Task<string> GetEmailConfirmationCode(String userId);
        Task<IdentityResult> ConfirmEmail(String userId, String Code);
        Task<string> GetResetPasswordCode(String userId);
        Task<IdentityResult> ResetPassword(String userId, String Code, string newPassword);
        bool ChangePassword(string UserId, string Password);
        object ChangePassword(string OldPassword, string NewPassword, string UserName);
        Boolean IsInRole(String RoleName, string userId);
    }
}
