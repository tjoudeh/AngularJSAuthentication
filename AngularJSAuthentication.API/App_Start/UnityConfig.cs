using System;
using System.Configuration;
using AngularJSAuthentication.Data.Interface;
using AngularJSAuthentication.Data.Repository;
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
            
        }






    }
}
