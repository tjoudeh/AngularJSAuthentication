using System.Linq;
using AngularJSAuthentication.API.Entities;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Provider;

namespace AngularJSAuthentication.API.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = GetClientId(context);

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (var _repo = new AuthRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>(Constants.Clients.RefreshTokeLifetimeKey); 
               
                var token = new RefreshToken 
                { 
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = clientid,
                    UserName = context.Ticket.Identity.Name,
                    UserAgent = GetUserAgent(context.OwinContext),
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)) 
                };

                CleanupStaleTokens(_repo, token);

                var result = _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
             
            }
        }

        private static string GetClientId(BaseContext context)
        {
            return context.OwinContext.Get<string>(Constants.Clients.ClientId);
        }

        private static string GetUserAgent(IOwinContext context)
        {
            return context.Request.Headers.Get("User-Agent").Substring(0, 50);
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(Constants.Clients.AllowedOrigin);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var hashedTokenId = Helper.GetHash(context.Token);

            using (var _repo = new AuthRepository())
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

                if (refreshToken == null)
                {
                    // Refresh token is not valid
                    // TODO: log suspicious activity
                    return;
                }
                if (refreshToken.ClientId != GetClientId(context))
                {
                    // Refresh token is issued to a different clientId.
                    // TODO: log suspicious activity
                    return;
                }
                var user = await _repo.FindUserByName(refreshToken.UserName);
                if (user == null)
                {
                    // Refresh token was issued for the other user
                    // TODO: log suspicious activity
                    return;
                }
                var ticket = AuthenticationTicketProvider.GetTicket(user, OAuthDefaults.AuthenticationType);
                // workaround for date expiry check in OAuthAuthorizationServerHandler.InvokeTokenEndpointRefreshTokenGrantAsync()
                // that will raise error if ticket.Properties.ExpiresUtc < currentUtc
                ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(10);

                // If you want to add any claims specificly for refresh tokens you can do that here
                //ticket.Identity.AddClaim(new Claim("newClaim", "newClaimValue"));
                context.SetTicket(ticket);
            }
        }


        private static void CleanupStaleTokens(AuthRepository repo, RefreshToken token)
        {
            var previousTokens = repo.FindRefreshTokens(token.ClientId, token.UserName, token.UserAgent);
            var staleTokens = repo.FindRefreshTokens(token.ClientId, token.UserName, null, DateTime.UtcNow);
            repo.RemoveRefreshToken(previousTokens.Union(staleTokens).ToArray());
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