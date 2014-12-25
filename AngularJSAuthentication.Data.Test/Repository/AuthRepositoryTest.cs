using System;
using System.Configuration;
using AngularJSAuthentication.API.Data;
using AngularJSAuthentication.Common.DI;
using AngularJSAuthentication.Data.Interface;
using AngularJSAuthentication.Data.Repository;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace AngularJSAuthentication.Data.Test.Repository
{
    [TestFixture]
    public class AuthRepositoryTest
    {
        private IAuthRepository authRepository;

        [SetUp]
        public void Setup()
        {
            var container = Ioc.Container;
            string connectionString = ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;

            container.RegisterType<IClientRepository, ClientRepository>(new InjectionConstructor(connectionString));
            container.RegisterType<IAuthRepository, MongoAuthRepository>();

            Ioc.Container.Resolve<IClientRepository>();
            authRepository = Ioc.Container.Resolve<IAuthRepository>();
        }


        [Test]
        public void Can_Get_Client_By_Id()
        {
            var client = authRepository.FindClient("ngAuthApp");
            Console.WriteLine(client.Name);
        }


    }
}
