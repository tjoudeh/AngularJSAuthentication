using System.Collections.Generic;
using System.Configuration;
using AngularJSAuthentication.Common.DI;
using AngularJSAuthentication.Common.Helpers;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Interface;
using AngularJSAuthentication.Data.Models;
using AngularJSAuthentication.Data.Repository;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace AngularJSAuthentication.Data.Test.Repository
{
    [TestFixture]
    public class ClientRepositoryTest
    {
        private IClientRepository clientRepoistory;

        [SetUp]
        public void Setup()
        {
            var container = Ioc.Container;
            string connectionString = ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;

            container.RegisterType<IClientRepository, ClientRepository>(new InjectionConstructor(connectionString));
            clientRepoistory = Ioc.Container.Resolve<IClientRepository>();
        }

        [Test]
        public void Can_Create_Clients()
        {
                
            
        }

        private static List<Client> BuildClientsList()
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
                    AllowedOrigin = "http://ngauthenticationweb.azurewebsites.net"
                },
                new Client
                { Id = "consoleApp", 
                    Secret= CryptographyHelper.GetHash("123@abc"), 
                    Name= "Console Application", 
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
