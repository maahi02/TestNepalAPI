using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
//using TestNepal.Entities;
using TestNepal.Repository.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TestNepal.Repository.Infrastructure;
using TestNepal.Repository.Repositories;
using System.Configuration;
using TestNepal.Api.Helpers;
using TestNepal.Dtos;

namespace TestNepal.Api.Providers
{
    public class FacliappRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static IRefreshTokenRepository _refreshTokenRepository;
        private static IUserRepository _userRepository;
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            InitilizeRepository();
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");


            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new TestNepal.Entities.RefreshToken()
            {
                Id = TestNepal.Dtos.Common.GetHash(refreshTokenId),
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();
            TestNepal.Entities.RefreshToken refreshToken= _refreshTokenRepository.Get(r => r.Subject == token.Subject && r.ClientId == token.ClientId);
            if (refreshToken != null)
            {
                _refreshTokenRepository.Delete(refreshToken);
                _refreshTokenRepository.SaveChanges();
            }
            _refreshTokenRepository.Add(token);
            _refreshTokenRepository.SaveChanges();

            context.SetToken(refreshTokenId);
            
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            InitilizeRepository();
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = TestNepal.Dtos.Common.GetHash(context.Token);


            var refreshToken =  _refreshTokenRepository.GetById(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                _refreshTokenRepository.Delete(refreshToken);
                _refreshTokenRepository.SaveChanges();
            }
        }
        public static void InitilizeRepository()
        {
            DbFactory _dbFactory = new DbFactory(new Guid(ConfigurationManager.AppSettings[Constants.API_CLAIM_TYPE_TENANT_ID]), Guid.Empty);
            _refreshTokenRepository = new RefreshTokenRepository(_dbFactory);
            _userRepository = new UserRepository(_dbFactory);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}