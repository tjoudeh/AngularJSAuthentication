using System;
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
    public class RefreshTokenRepositoryTest
    {
        private IRefreshTokenRepository refreshTokenRepository;

        [SetUp]
        public void Setup()
        {
            var container = Ioc.Container;
            string connectionString = ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString;

            container.RegisterType<IRefreshTokenRepository, RefreshTokenRepository>(new InjectionConstructor(connectionString));
            refreshTokenRepository = Ioc.Container.Resolve<IRefreshTokenRepository>();
        }

        [Test]
        public void Can_Create_Refresh_Token()
        {
            var token = BuildRefreshToken();
            refreshTokenRepository.AddRefreshToken(token);
        }

        private static RefreshToken BuildRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Subject = "TestSubject",
                ClientId = "ngAuthApp",
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(90),
                ProtectedTicket = "ca7e44ad-533f-481e-b492-376c2f1b2595",
            };

            return refreshToken;
        }

    }
}
