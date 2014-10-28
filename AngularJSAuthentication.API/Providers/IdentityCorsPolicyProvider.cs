using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Cors;
using AngularJSAuthentication.API.Models;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;

namespace AngularJSAuthentication.API.Providers
{
    public class IdentityCorsPolicyProvider : CorsPolicyProvider
    {
        public const char AllowedOriginSeparator = ',';


        public override async Task<CorsPolicy> GetCorsPolicyAsync(IOwinRequest request)
        {
            var policy = new CorsPolicy();
            var corsContext = GetCorsRequestContext(request.Context);
            if (corsContext == null)
            {
                return await Task.FromResult(policy);
            }

            var allowedOrigins = new string[] { };

            if (corsContext.IsPreflight)
            {
                // For OPTIONS request (which will not have client_id) we'll get all allowed origins
                // from the Clients table and CorsEngine would match those in 'Origin' request header
                using (var repo = new AuthRepository())
                {
                    var allAllowedOrigins = repo.GetAllClients(ApplicationTypes.JavaScript).Select(t => t.AllowedOrigin).ToList();
                    allowedOrigins = allAllowedOrigins.SelectMany(t => t.Split(AllowedOriginSeparator).Select(x => x.Trim())).ToArray();
                }
            }
            else
            {
                // For normal CORS request we'll resolve Client by client_id and if client is a web
                // app than we'll allow all origins from client.AllowedOrigins 
                var clientId = GetClientId(request);
                using (var repo = new AuthRepository())
                {
                    var client = repo.FindClient(clientId);

                    if (client != null &&
                        client.ApplicationType == ApplicationTypes.JavaScript &&
                        !string.IsNullOrEmpty(client.AllowedOrigin))
                    {
                        allowedOrigins = client.AllowedOrigin
                            .Split(AllowedOriginSeparator)
                            .Select(t => t.Trim()).ToArray();
                    }
                }
            }

            SetAllowedOrigins(allowedOrigins, policy);

            // setting flag for other Owin middleware so that OAuthServerProvider can terminate if the flag is false
            var isOriginAllowed = policy.AllowAnyOrigin || policy.Origins.Contains(corsContext.Origin);
            request.Context.Set(Constants.OAuth.IsOriginAllowed, isOriginAllowed);

            return await Task.FromResult(policy);
        }

        /// <summary>
        /// Resolving client_id from request context by wrapping request OAuthValidateClientAuthenticationContext
        /// and reusing TryGetBasicCredentials and TryGetFormCredentials methods
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetClientId(IOwinRequest request)
        {
            string clientId, clientSecret;
            var form = request.ReadFormAsync().Result;
            var clientContext = new OAuthValidateClientAuthenticationContext(request.Context, null, form);
            clientContext.GetClientCredentials(out clientId, out clientSecret);
            return clientId;
        }

        /// <summary>
        /// Adding allowed origins to CorsPolicy.Origin collection or specifying 
        /// policy.AllowAnyOrigin if one of the origins is '*'
        /// </summary>
        /// <param name="allowedOrigins"></param>
        /// <param name="policy"></param>
        private static void SetAllowedOrigins(ICollection<string> allowedOrigins, CorsPolicy policy)
        {
            if (allowedOrigins.Contains(CorsConstants.AnyOrigin))
            {
                policy.AllowAnyOrigin = true;
            }
            else
            {
                foreach (var origin in allowedOrigins)
                {
                    policy.Origins.Add(origin);
                }
            }
        }

        /// <summary>
        /// Here we are mimicking process of getting the same CorsRequestContext an in Microsoft.Owin.Cors.CorsMiddleware
        /// pipeline. This is needed mostly for doing 'if (corsContext.IsPreflight)' check. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static CorsRequestContext GetCorsRequestContext(IOwinContext context)
        {
            var origin = context.Request.Headers.Get(CorsConstants.Origin);

            if (String.IsNullOrEmpty(origin))
            {
                return null;
            }

            var requestContext = new CorsRequestContext
            {
                RequestUri = context.Request.Uri,
                HttpMethod = context.Request.Method,
                Host = context.Request.Host.Value,
                Origin = origin,
                AccessControlRequestMethod = context.Request.Headers.Get(CorsConstants.AccessControlRequestMethod)
            };

            var headerValues = context.Request.Headers.GetCommaSeparatedValues(CorsConstants.AccessControlRequestHeaders);

            if (headerValues != null)
            {
                foreach (var header in headerValues)
                {
                    requestContext.AccessControlRequestHeaders.Add(header);
                }
            }

            return requestContext;
        }
    }
}