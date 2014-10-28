using Microsoft.Owin.Security.Facebook;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AngularJSAuthentication.API.Providers
{
    public class FacebookAuthProvider : FacebookAuthenticationProvider
    {
        public override Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }
    }
}