using System;
using System.Configuration;
using AngularJSAuthentication.API.Data;
using AngularJSAuthentication.Data.Entities;
using AngularJSAuthentication.Data.Interface;
using AngularJSAuthentication.Data.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;

namespace AngularJSAuthentication.API.App_Start
{

    public class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;

            container.RegisterType<IClientRepository, ClientRepository>(new InjectionConstructor(connectionString));
            container.RegisterType<IRefreshTokenRepository, RefreshTokenRepository>(new InjectionConstructor(connectionString));
            container.RegisterType<IUserRepository<User>, UserRepository<User>>(new InjectionConstructor(connectionString));
            container.RegisterType<IUserStore<User>, UserRepository<User>>();

            //container.RegisterType<IAuthRepository, MongoAuthRepository>();
            


            container.RegisterType<IdentityDbContext<IdentityUser>, AuthContext>(new HierarchicalLifetimeManager());

        }

    }
}
