// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;

namespace NavigatorGlass.Owin.Security.Provider
{
  
    public class GlassAuthenticatedContext : BaseContext
    {
 
        public GlassAuthenticatedContext(IOwinContext context, JObject user, string accessToken, string refreshToken, string tokenType, long expiresin, DateTime issued): base(context)
        {
            User = user;
            AccessToken = accessToken;
            Id = TryGetValue(user, "sub");
            Name = TryGetValue(user, "name");
            GivenName = TryGetValue(user, "given_name");
            FamilyName = TryGetValue(user, "family_name");
            Profile = TryGetValue(user, "profile");
            Email = TryGetValue(user, "email");
            RefreshToken = refreshToken;
            ExpiresIn = expiresin;
            TokenType = tokenType;
            Issued = issued;
        }

        public JObject User { get; private set; }
        public DateTime Issued { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public long ExpiresIn { get; set; }
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Profile { get; private set; }
        public string Email { get; private set; }
        public ClaimsIdentity Identity { get; set; }
        public AuthenticationProperties Properties { get; set; }
        private static string TryGetValue(JObject user, string propertyName)
        {
            JToken value;
            return user.TryGetValue(propertyName, out value) ? value.ToString() : null;
        }
    }
}
