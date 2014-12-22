using AngularJSAuthentication.Common.Helpers;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Models;

namespace AngularJSAuthentication.API.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AngularJSAuthentication.API.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AngularJSAuthentication.API.AuthContext context)
        {
            if (context.Clients.Any())
            {
                return;
            }

            context.Clients.AddRange(BuildClientsList());
            context.SaveChanges();
        }

        private static IEnumerable<Client> BuildClientsList()
        {
            var ClientsList = new List<Client> 
            {
                new Client
                { Id = "ngAuthApp", 
                    Secret= CryptographyHelper.GetHash("abc@123"), 
                    Name="AngularJS front-end Application", 
                    ApplicationType =  ApplicationTypes.JavaScript, 
                    Active = true, 
                    RefreshTokenLifeTime = 7200, 
                    //AllowedOrigin = "http://ngauthenticationweb.azurewebsites.net"
                    AllowedOrigin = "*"
                },
                new Client
                { Id = "consoleApp", 
                    Secret= CryptographyHelper.GetHash("123@abc"), 
                    Name="Console Application", 
                    ApplicationType = ApplicationTypes.NativeConfidential, 
                    Active = true, 
                    RefreshTokenLifeTime = 14400, 
                    AllowedOrigin = "*"
                }
            };

            return ClientsList;
        }
    }
}
