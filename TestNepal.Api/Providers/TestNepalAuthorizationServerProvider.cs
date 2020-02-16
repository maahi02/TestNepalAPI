using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TestNepal.Repository.Common;
using TestNepal.Repository.Infrastructure;
using TestNepal.Repository.Repositories;
using TestNepal.Service.Infrastructure;
using TestNepal.Service;
using TestNepal.Api.Helpers;
using System.Configuration;
using TestNepal.Dtos;

namespace TestNepal.Api.Providers
{
    public class TestNepalAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private static IClientRepository _clientRepository;
        private static UserService _userService;
        private static IUserProfileRepository _userProfileRepository;
        private String FirebaseToken = "";
        private String DeviceType = "";
        private Boolean AcceptTC = false;
        private Boolean IsWeb = false;
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;
            TestNepal.Entities.Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            FirebaseToken = context.Parameters.Get("devicetoken");
            DeviceType = context.Parameters.Get("devicetype");
            
            if (context.Parameters.Get("accepttc") != null)
            {
                AcceptTC = context.Parameters.Get("accepttc").ToLower() == "true" ? true : false;
            }
            if (context.Parameters.Get("isweb") != null)
            {
                IsWeb = context.Parameters.Get("isweb").ToLower() == "true" ? true : false;
            }
            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                //context.Validated();
                context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(context.Parameters);
            }

            InitilizeRepository(new Guid(ConfigurationManager.AppSettings[Constants.API_CLAIM_TYPE_TENANT_ID]));
            client = _clientRepository.GetById(context.ClientId);


            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == TestNepal.Entities.ApplicationType.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != TestNepal.Dtos.Common.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";
            var header = context.OwinContext.Response.Headers.SingleOrDefault(h => h.Key == "Access-Control-Allow-Origin");
            if (header.Equals(default(KeyValuePair<string, string[]>)))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            }

            InitilizeRepository(new Guid(ConfigurationManager.AppSettings[Constants.API_CLAIM_TYPE_TENANT_ID]));
            IdentityUser user = new IdentityUser();
            string contextUserName = context.UserName;
            user = await _userService.FindUser(contextUserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            if(user.EmailConfirmed == false)
            {
                context.SetError("invalid_grant", "Please verify your email.");
                return;
            }
            if (IsWeb == true) {
                if ((_userService.IsInRole("FRANCHISEE", user.Id) ? "FRANCHISEE" : "CUSTOMER") == "FRANCHISEE")
                {
                    context.SetError("invalid_grant", "Only customer is allowed to login.");
                    return;
                }
            }

            TestNepal.Entities.UserProfile profile = _userProfileRepository.Get(a => a.User.Id == user.Id);

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, contextUserName));
            if (user.Roles.Count > 0)
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault().RoleId));
            identity.AddClaim(new Claim(Constants.API_CLAIM_TYPE_TENANT_ID, ConfigurationManager.AppSettings[Constants.API_CLAIM_TYPE_TENANT_ID]));
            identity.AddClaim(new Claim(Constants.API_CLAIM_TYPE_USER_ID, user.Id.ToString()));
            identity.AddClaim(new Claim(Constants.TestNepal_FIRSTNAME, profile != null && profile.FirstName != null ? profile.FirstName:""));
            identity.AddClaim(new Claim(Constants.TestNepal_LASTNAME, profile != null && profile.LastName != null ? profile.LastName : ""));
            identity.AddClaim(new Claim(ClaimTypes.Email, profile != null && profile.Email != null ? profile.Email : ""));
            identity.AddClaim(new Claim(ClaimTypes.MobilePhone, profile != null && profile.Phone != null ? profile.Phone : ""));
            
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    {
                        "userName", contextUserName
                    },
                    {
                        "profileImage", profile != null && String.IsNullOrEmpty(profile.Photo) == false ?  ApplicationSettingVariables.WebsiteBaseUrl+ ApplicationSettingVariables.ImageUploadPath+ profile.Photo : ""
                    }
                    ,
                    {
                        "name", profile != null ? profile.FirstName+" "+profile.LastName:""
                    },
                    {
                        "userType", _userService.IsInRole("FRANCHISEE",user.Id) ?"FRANCHISEE":"CUSTOMER"
                    }

                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public static void InitilizeRepository(Guid tenantId)
        {
            DbFactory _dbFactory = new DbFactory(tenantId, Guid.Empty);
            _clientRepository = new ClientRepository(_dbFactory);
            UnitOfWork unitOfWork = new UnitOfWork(_dbFactory);
            UserRepository userRepository = new UserRepository(_dbFactory);
            _userProfileRepository = new UserProfileRepository(_dbFactory);
            _userService = new UserService(userRepository, _userProfileRepository, unitOfWork);
        }
    }
}