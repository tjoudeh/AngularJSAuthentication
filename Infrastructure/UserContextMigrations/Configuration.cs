using System.Collections.Generic;
using System.Linq;
using Core.DomainModel.AuthEntities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Infrastructure.UserContextMigrations
{
    using System.Data.Entity.Migrations;

    public class Configuration : DbMigrationsConfiguration<Infrastructure.Login.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"UserContextMigrations";
        }

        protected override void Seed(Infrastructure.Login.AuthContext context)
        {

            if (context.Users.FirstOrDefault(user => user.UserName == "Admin") == null)
                context.Users.Add(new IdentityUser
                {
                    UserName = "Admin",
                    PasswordHash = "AAnCjncmdIyCvpylYGBcWsZeTiRWH/U5Zpzfc8mX+mlMbUBImctqc2cpzuhG4Xpdyg==" //123456
                });

            if (!context.Clients.Any()) context.Clients.AddRange(BuildClientsList());
            
            context.SaveChanges();
        }

        private static IEnumerable<Client> BuildClientsList()
        {

            var clientsList = new List<Client> 
            {
                new Client
                { 
                    Id = "ngAuthApp", 
                    Secret = Helper.GetHash("abc@123"), 
                    Name="AngularJS front-end Application", 
                    ApplicationType = ApplicationTypes.JavaScript, 
                    Active = true, 
                    RefreshTokenLifeTime = 7200, 
                    //Change this to real URL
                    AllowedOrigin = "https://localhost:44301"
                },
                new Client
                { 
                    Id = "consoleApp", 
                    Secret= Helper.GetHash("123@abc"), 
                    Name="Console Application", 
                    ApplicationType = ApplicationTypes.NativeConfidential, 
                    Active = true, 
                    RefreshTokenLifeTime = 14400, 
                    AllowedOrigin = "*"
                }
            };

            return clientsList;
        }
    }
}
