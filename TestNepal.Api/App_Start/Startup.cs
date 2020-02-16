using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using Hangfire;


using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using TestNepal.Api.Providers;
using Unity.Injection;
using System.Data.Entity.Infrastructure;
using TestNepal.Repository.Common;
using TestNepal.Api.Helpers;
using Unity;
using AutoMapper;
using TestNepal.API.Mappers;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

[assembly: OwinStartup(typeof(TestNepal.Api.Startup))]

namespace TestNepal.Api
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(Context.TestNepalContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, Entities.ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            HttpConfiguration config = new HttpConfiguration();
            var container = new UnityContainer();
            
            ConfigureOAuth(app);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //WebApiConfig.Register(config);
            
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //app.use(config);
            app.UseWebApi(config);

            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
            container.RegisterInstance(typeof(IMapper), new Mapper(mapperConfig));

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context.TestNepalContext, Repository.Migrations.Configuration>());

            using (Context.TestNepalContext context = new Context.TestNepalContext())
            {
                context.Database.Initialize(true);
            }
            //Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("TestNepalDBConnection");
            //app.UseHangfireDashboard("/webjob", new DashboardOptions() {
            //    Authorization = new[] {new Helpers.HangfireAuthorizationFilter()}
            //});
            //app.UseHangfireServer();
            Context.TestNepalContextSeedData seeder = new Context.TestNepalContextSeedData(new Context.TestNepalContext());
            seeder.SeedDefaultDatas();
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),// TimeSpan.FromMinutes(30),
                Provider = new TestNepalAuthorizationServerProvider(),
                RefreshTokenProvider = new FacliappRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            //Configure Google External Login
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "xxxxxx",
                ClientSecret = "xxxxxx",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "XXXX",
                AppSecret = "XXXXX",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

        }
    }
}
