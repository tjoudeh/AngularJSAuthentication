// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using NavigatorGlass.Owin.Security;
using Owin;

namespace AngularJSAuthentication.Glass
{
    public static class GlassAuthenticationExtensions
    {
        public static IAppBuilder UseGlassAuthentication(this IAppBuilder app, GlassAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(GlassAuthenticationMiddleware), app, options);
            return app;
        }
        public static IAppBuilder UseGlassAuthentication(
            this IAppBuilder app,
            string clientId,
            string clientSecret)
        {
            return UseGlassAuthentication(
                app,
                new GlassAuthenticationOptions
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                });
        }
    }
}
