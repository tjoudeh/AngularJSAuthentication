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
        private IUserStore<User> userStore;
            
        [SetUp]
        public void Setup()
        {
            var container = Ioc.Container;

            string connectionString = ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;
            container.RegisterType<IUserRepository<User>, UserRepository<User>>(new InjectionConstructor(connectionString));
            container.RegisterType<IUserStore<User>, UserRepository<User>>();

            Ioc.Container.Resolve<IUserRepository<User>>();
            userStore = Ioc.Container.Resolve<IUserStore<User>>();
           
        }

        [Test]
        [TestCase("5419160d57dd4f0b7cbdd9f1")] //StuartShay
        public void Can_Get_Credentials_By_Id(string id)
        {
            var user = userStore.FindByIdAsync(id).Result;

            System.Console.WriteLine("UserId:" + user.Id);
            System.Console.WriteLine("User Name:" + user.UserName);
            System.Console.WriteLine("UserId:" + user.UserId);

            Assert.AreEqual("StuartShay", user.UserName);
        }


        [Test]
        [TestCase("StuartShay")]  //StuartShay
        public void Can_Get_Credentials_By_UserName(string userName)
        {
            var user = userStore.FindByNameAsync(userName).Result;      
            System.Console.WriteLine("User Name:" + user.UserName);
            System.Console.WriteLine("UserId:" + user.UserId);

            Assert.AreEqual(userName, user.UserName);
        }

        [Test]
        public void Can_Create_User()
        {
            var user = new User
            {
              UserId = "110997167815875453301",
              UserName = "Nandini",
            };

            userStore.CreateAsync(user);

        }

        [Test]
        public void Can_Delete_User()
        {
            var user = new User
            {
                Id = "549799bd5c9599667c625287",
                UserId = "110997167815875453301",
                UserName = "Nandini",
            };

            userStore.DeleteAsync(user);

        }

    }
}
