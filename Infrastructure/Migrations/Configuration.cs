using System.Collections.Generic;
using Core.DomainModel.AuthEntities;

namespace Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Infrastructure.Login.AuthContext>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = false; MAYBE?
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Infrastructure.Login.AuthContext context)
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

            var clientsList = new List<Client> 
            {
                new Client
                { 
                    Id = "ngAuthApp", 
                    Secret= Helper.GetHash("abc@123"), 
                    Name="AngularJS front-end Application", 
                    ApplicationType =  ApplicationTypes.JavaScript, 
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
