using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace AngularJSAuthentication.API.Providers
{
    public class AuthenticationTicketProvider
    {

        public static AuthenticationTicket GetTicket(IdentityUser user, string authType)
        {
            var identity = new ClaimsIdentity(authType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", user.UserName));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {"userName", user.UserName}
            });

            var ticket = new AuthenticationTicket(identity, props);
            return ticket;
        }
    }
}