using System.Configuration;
using AngularJSAuthentication.Common.DI;
using AngularJSAuthentication.Data.Interface;
using AngularJSAuthentication.Data.Repository;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace AngularJSAuthentication.Data.Test.Repository
{
    [TestFixture]
    public class ClientRepoistoryTest
    {
        private IClientRepoistory clientRepoistory;

        [SetUp]
        public void Setup()
        {
            var container = Ioc.Container;
            string connectionString = ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;

            container.RegisterType<IClientRepoistory, ClientRepoistory>(new InjectionConstructor(connectionString));
            clientRepoistory = Ioc.Container.Resolve<IClientRepoistory>();
        }



    }





}
