using System;
using System.Security.Cryptography;
using Microsoft.Owin.Security.OAuth;

namespace AngularJSAuthentication.API
{
    public static class Helper
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
       
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static void GetClientCredentials(this OAuthValidateClientAuthenticationContext context, out string clientId, out string clientSecret)
        {
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
        }

        public static string GetUniqueId()
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}