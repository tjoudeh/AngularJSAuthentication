using System;
using System.Configuration;
using AngularJSAuthentication.Common.DI;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Interface;
using AngularJSAuthentication.Data.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace AngularJSAuthentication.Data.Test.Repository
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private IUserStore<User> userRepository;

        [SetUp]
        public void Setup()
        {
            var container = Ioc.Container;
            string connectionString = ConfigurationManager.ConnectionStrings["MongoCredentials"].ConnectionString;
            container.RegisterType<IUserStore<User>, UserRepository<User>>(new InjectionConstructor(connectionString));

            userRepository = Ioc.Container.Resolve<IUserStore<User>>();
        }

        [Test]
        [TestCase("116590040434310834456")]  //StuartShay
        public void Can_Get_Credentials_By_UserId(string userId)
        {
            var user = userRepository.FindByIdAsync(userId).Result;  
            System.Console.WriteLine("User Name:" + user.UserName);
            System.Console.WriteLine("UserId:" + user.UserId);




            //Assert.AreEqual(user.UserId, userId);
        }






    }
}
