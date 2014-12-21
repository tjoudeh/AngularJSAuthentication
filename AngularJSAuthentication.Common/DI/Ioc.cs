using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;


namespace AngularJSAuthentication.Common.DI
{
    public class Ioc : IUnityContainer
    {
        private static IUnityContainer container = new UnityContainer();

        private static bool isInitialized = false;

        private static readonly object lockObject = new object();

        private Ioc()
        {
            // prevent from intialize new instance
        }

        public static IUnityContainer Container
        {
            get
            {
                if (!isInitialized)
                {
                    lock (lockObject)
                    {
                        container = new UnityContainer();
                        isInitialized = true;
                    }
                }

                return container;
            }
        }

        public IUnityContainer Parent
        {
            get { return Container.Parent; }
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get { return Container.Registrations; }
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return Container.AddExtension(extension);
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return Container.BuildUp(t, existing, name, resolverOverrides);
        }

        public object Configure(Type configurationInterface)
        {
            return Container.Configure(configurationInterface);
        }

        public IUnityContainer CreateChildContainer()
        {
            return Container.CreateChildContainer();
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            return Container.RegisterInstance(t, name, instance, lifetime);
        }

        public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return Container.RegisterType(from, to, name, lifetimeManager, injectionMembers);
        }

        public IUnityContainer RemoveAllExtensions()
        {
            return Container.RemoveAllExtensions();
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return Container.Resolve(t, name, resolverOverrides);
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return Container.ResolveAll(t, resolverOverrides);
        }

        public void Teardown(object o)
        {
            Container.Teardown(o);
        }

        public void Dispose()
        {
            Container.Dispose();
        }


    }
}
