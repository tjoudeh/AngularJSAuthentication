using AngularJSAuthentication.API.Entities;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace AngularJSAuthentication.API.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.OwinContext.Get<string>("as:client_id");

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (var _repo = new AuthRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime"); 
               
                var token = new RefreshToken 
                { 
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = clientid, 
                    UserName = context.Ticket.Identity.Name,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)) 
                };
                
                var result = await _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
             
            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var hashedTokenId = Helper.GetHash(context.Token);

            using (var _repo = new AuthRepository())
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {

                    var user = await _repo.FindUserByName(refreshToken.UserName);
                    if (user != null)
                    {
                        var ticket = AuthenticationTicketProvider.GetTicket(user, OAuthDefaults.AuthenticationType);
                        // workaround for date expiry check in OAuthAuthorizationServerHandler.InvokeTokenEndpointRefreshTokenGrantAsync()
                        // that will raise error if ticket.Properties.ExpiresUtc < currentUtc
                        ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(10);
                        context.SetTicket(ticket);

                        await _repo.RemoveRefreshToken(hashedTokenId);
                    }
                    else
                    {
                        // TODO: log suspicious activity
                    }
                }
                else
                {
                    // TODO: log suspicious activity
                }
            }
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