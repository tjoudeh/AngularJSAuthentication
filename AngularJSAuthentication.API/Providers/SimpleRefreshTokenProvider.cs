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

        public void Create(AuthenticationTokenCreateContext context)
        {
            var clientid = GetClientId(context);

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Helper.GetUniqueId();
            var refreshTokenLifeTime = context.OwinContext.Get<string>(Constants.OAuth.RefreshTokeLifeTime);
            var userAgentId = context.OwinContext.Get<string>(Constants.OAuth.UserAgentId);

            var token = new RefreshToken
            {
                Id = Helper.GetHash(refreshTokenId),
                ClientId = clientid,
                UserName = context.Ticket.Identity.Name,
                UserAgent = GetUserAgent(context.OwinContext),
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)),
                UserAgentId = userAgentId,
            };

            using (var _repo = new AuthRepository())
            {
                CleanupRefreshTokens(_repo, token);
                if (_repo.AddRefreshToken(token))
                {
                    context.SetToken(refreshTokenId);
                }
            }
        }

        private static string GetClientId(BaseContext context)
        {
            return context.OwinContext.Get<string>(Constants.OAuth.ClientId);
        }

        private static string GetUserAgent(IOwinContext context)
        {
            return context.Request.Headers.Get("User-Agent").Substring(0, 50);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            var hashedTokenId = Helper.GetHash(context.Token);

            using (var _repo = new AuthRepository())
            {
                var refreshToken = _repo.FindRefreshToken(hashedTokenId);

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
                var user = _repo.FindUserByName(refreshToken.UserName);
                if (user == null)
                {
                    // Refresh token was issued for the other user
                    // TODO: log suspicious activity
                    return;
                }
                context.OwinContext.Set(Constants.OAuth.UserAgentId, refreshToken.UserAgentId);
                var ticket = AuthenticationTicketProvider.GetTicket(user, OAuthDefaults.AuthenticationType);
                // workaround for date expiry check in OAuthAuthorizationServerHandler.InvokeTokenEndpointRefreshTokenGrantAsync()
                // that will raise error if ticket.Properties.ExpiresUtc < currentUtc
                ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(10);

                // If you want to add any claims specificly for refresh tokens you can do that here
                //ticket.Identity.AddClaim(new Claim("newClaim", "newClaimValue"));
                context.SetTicket(ticket);
            }
        }


        private static void CleanupRefreshTokens(AuthRepository repo, RefreshToken token)
        {
            // deleting previous refrehs tokens for same user-agents
            var previousTokens = repo.FindRefreshTokens(token.ClientId, token.UserName, token.UserAgentId).ToArray();
            repo.RemoveRefreshToken(previousTokens);

            // deliting expired tokens for the current user
            var expiredTokens = repo.FindRefreshTokens(token.ClientId, token.UserName, null, DateTime.UtcNow).ToArray();
            repo.RemoveRefreshToken(expiredTokens);
        }


        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            Create(context);
            return Task.FromResult<object>(null);
        }

        public Task ReceiveAsync (AuthenticationTokenReceiveContext context)
        {
            Receive(context);
            return Task.FromResult<object>(null);
        }
    }
}