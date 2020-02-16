using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNepal.Entities;
using TestNepal.Service.Infrastructure;
using TestNepal.Repository.Infrastructure;
using TestNepal.Repository.Common;
using System.Linq.Expressions;
using TestNepal.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestNepal.Dtos;

namespace TestNepal.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IUserProfileRepository _userProfileRepository;
        IUnitOfWork _unitOfWork;
        private TestNepalContext _ctx;
        private UserManager<IdentityUser> _userManager;
        public UserService(IUserRepository userRepository, IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
            _ctx = new TestNepalContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));

        }
        public async Task<IdentityResult> RegisterUser(ApplicationUser aUser, String Password)
        {
            var result = await _userManager.CreateAsync(aUser, Password);
            return result;
        }

        /*
        public ApplicationUser GenerateRecoverPassword(ForgotPasswordModel model)
        {

            ApplicationUser user = (ApplicationUser)_userManager.FindByName(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return null;
            }
            UserProfile userProfile = _ctx.UserProfiles
                .Include("ResetPasswords")
                .FirstOrDefault(up => up.UserName == user.UserName);
            string password = RandomPasswordHelper.Generate(5);
            if (userProfile.ResetPasswords.Count > 5)
            {
                userProfile.ResetPasswords.Remove(userProfile.ResetPasswords.First());
            }
            userProfile.ResetPasswords.Add(new ResetPassword { PasswordToken = password });
            //await _ctx.SaveChangesAsync();
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }

            user.GeneratedResetCode = password;
            return user;

        }
        
        public bool ResetPassword(UserResetModel model)
        {
            UserProfile userProfile = null;


            using (AuthContext context = new AuthContext())
            {
                userProfile = context.UserProfiles
               .Include("User")
               .Include("ResetPasswords")
               .FirstOrDefault(up => up.UserName == model.UserName && up.ResetPasswords.Any(rp => rp.PasswordToken == model.Code));
                if (userProfile == null)
                {
                    return false;
                }
                context.ResetPasswords.RemoveRange(userProfile.ResetPasswords.ToList());
                context.SaveChanges();
                //foreach (var item in userProfile.ResetPasswords)
                //{
                //    _ctx.ResetPasswords.Remove(item);
                //}

            }
            //_userManager.RemovePassword(userProfile.User.Id);
            //userManager.AddPassword(userProfile.User.Id, model.NewPassword);
            IdentityUser user = _userManager.FindById(userProfile.User.Id);
            if (user == null)
            {
                return false;
            }
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(model.NewPassword);
            var result = _userManager.Update(user);

            return true;

        }

       
*/

        /*
        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _ctx.RefreshTokens.ToList();
        }
        */
        public async Task<string> GetEmailConfirmationCode(String userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("TestNepal");
            _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser, String>(provider.Create("EmailConfirmation"));
            string confrimedEmailCode = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            return confrimedEmailCode;
        }
        public async Task<IdentityResult> ConfirmEmail(String userId, String Code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(Code))
            {
                return null;
            }
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("TestNepal");
            _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser, String>(provider.Create("EmailConfirmation"));
            var result = await _userManager.ConfirmEmailAsync(user.Id, Code);
            return result;
        }

        public async Task<string> GetResetPasswordCode(String userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("TestNepal");
            _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser, String>(provider.Create("PasswordReset"));
            string PasswordResetCode = await _userManager.GeneratePasswordResetTokenAsync(userId);
            return PasswordResetCode;
        }

        public async Task<IdentityResult> ResetPassword(String userId, String Code, string newPassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(Code))
            {
                return null;
            }
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("TestNepal");
            _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser, String>(provider.Create("PasswordReset"));
            var result = await _userManager.ResetPasswordAsync(user.Id, Code, newPassword);
            return result;
        }
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<IdentityUser> FindUser(string userName)
        {
            IdentityUser user = await _userManager.FindByNameAsync(userName);
            return user;
        }
        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(UserRegistration user)
        {
            if(_userManager.FindByEmail(user.Email) == null)
            {
                var aUser = new ApplicationUser()
                {
                    UserName = user.Email,
                    Email = user.Email,
                    PasswordHash = _userManager.PasswordHasher.HashPassword(user.Password),
                    UserProfile = new UserProfile()
                    {
                        UserName = user.Email,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        TenantId = user.TenantId.Value,
                        CreatedOn = DateTime.Now
                    }
                };

                var result = await _userManager.CreateAsync(aUser);
                await _userManager.AddToRoleAsync(aUser.Id, "CUSTOMER");
                user.Id = aUser.Id;

               
                UserProfile userProfile = _userProfileRepository.Get(a => a.UserName == user.Email);
                userProfile.UserId = new Guid(user.Id);
                _userProfileRepository.SaveChanges();
                return result;
            } else
            {
                IdentityResult result = new IdentityResult("Email already registered.");
                return result;
            }
        }
        public IEnumerable<ApplicationUser> GetAll(Expression<Func<ApplicationUser, bool>> where = null, params Expression<Func<ApplicationUser, object>>[] includeExpressions)
        {
            return _userRepository.GetMany(where, includeExpressions);
        }
        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            try
            {
                var result = await _userManager.AddLoginAsync(userId, login);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public Boolean IsInRole(String RoleName,string userId)
        {
            IList<string> roles =  _userManager.GetRoles(userId);
            return roles.Contains(RoleName);
        }
        public void Dispose()
        {
            //_ctx.Dispose();
            _userManager.Dispose();

        }

        public void Login()
        {
            throw new NotImplementedException();
        }

        public string GetUserId(string UserName)
        {
            string userId = "";
            var user = _userManager.FindByName(UserName);
            if (user != null)
            {
                userId = user.Id.ToString();
            }
            return userId;
        }
        public string GetEmailById(string Id)
        {
            ApplicationUser applicationUser = _userRepository.Get(a => a.Id == Id);
            return applicationUser != null ? applicationUser.Email : "";
        }
        public bool ChangePassword(string UserId, string Password)
        {
            bool isSuccess = true;
            try
            {
                IdentityUser user = _userManager.FindById(UserId);
                if (user == null)
                {
                    return false;
                }
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(Password);
                var result = _userManager.Update(user);
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public object ChangePassword(string OldPassword, string NewPassword, string UserName)
        {
            try
            {
                IdentityUser user = _userManager.Find(UserName, OldPassword);
                if (user == null)
                {
                    return Common.JsonErrorObject("Old password do not match.");
                }
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(NewPassword);
                var result = _userManager.Update(user);
                if (result.Succeeded)
                    return Common.JsonOkObject(new { msg = "Password changed successfully." });
                else
                    return Common.JsonErrorObject("Password could not be changed. Please try again later.");
            }
            catch
            {
                return Common.JsonErrorObject("Password could not be changed. Please try again later.");
            }
        }
    }
}
