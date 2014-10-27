using AngularJSAuthentication.API.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace AngularJSAuthentication.API.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId, clientSecret;
            Client client;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (clientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (var _repo = new AuthRepository())
            {
                client = _repo.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == Models.ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else if (client.Secret != Helper.GetHash(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");
                    return Task.FromResult<object>(null);
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set(Constants.Clients.ClientId, clientId);
            context.OwinContext.Set(Constants.Clients.AllowedOrigin, client.AllowedOrigin);
            context.OwinContext.Set(Constants.Clients.RefreshTokeLifetimeKey, client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>(Constants.Clients.AllowedOrigin) ?? "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            IdentityUser user;
            using (var _repo = new AuthRepository())
            {
                user = await _repo.FindUser(context.UserName, context.Password);
            }
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
            }
            else
            {
                var ticket = AuthenticationTicketProvider.GetTicket(user, OAuthDefaults.AuthenticationType);
                context.Validated(ticket);
            }

        }
    }
}