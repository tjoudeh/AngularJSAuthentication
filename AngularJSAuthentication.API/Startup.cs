using System.Configuration;
using AngularJSAuthentication.API.App_Start;
using AngularJSAuthentication.API.Data;
using AngularJSAuthentication.API.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Data.Entity;
using System.Web.Http;

[assembly: OwinStartup(typeof(AngularJSAuthentication.API.Startup))]

namespace AngularJSAuthentication.API
{
    public class Startup
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string tokenEndpointPath;

        public Startup()
        {
            clientId = ConfigurationManager.AppSettings["ClientId"];
            clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            tokenEndpointPath = ConfigurationManager.AppSettings["tokenEndpointPath"]; 
        }

        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            
            var container = UnityConfig.GetConfiguredContainer();
            config.DependencyResolver = new UnityResolver(container);

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, AngularJSAuthentication.API.Migrations.Configuration>());
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var OAuthServerOptions = new OAuthAuthorizationServerOptions() {
            
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(tokenEndpointPath),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                
                //TODO Inject Providers 
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider(),

                //http://www.strathweb.com/2012/11/asp-net-web-api-and-dependencies-in-request-scope/
                //Provider = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOAuthAuthorizationServerProvider)) as IOAuthAuthorizationServerProvider,
                //RefreshTokenProvider = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IAuthenticationTokenProvider)) as IAuthenticationTokenProvider,
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            //Configure Google External Login
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Provider = new GoogleAuthProvider()
            };

            app.UseGoogleAuthentication(googleAuthOptions);

        }
    }
}