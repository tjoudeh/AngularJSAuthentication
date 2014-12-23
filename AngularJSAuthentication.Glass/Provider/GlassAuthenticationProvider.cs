// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace NavigatorGlass.Owin.Security.Provider
{
    public class GlassAuthenticationProvider : IGlassAuthenticationProvider
    {
        public GlassAuthenticationProvider()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);
            OnReturnEndpoint = context => Task.FromResult<object>(null);
        }
        public Func<GlassAuthenticatedContext, Task> OnAuthenticated { get; set; }
        public Func<GlassReturnEndpointContext, Task> OnReturnEndpoint { get; set; }
        public virtual Task Authenticated(GlassAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }
        public virtual Task ReturnEndpoint(GlassReturnEndpointContext context)
        {
            return OnReturnEndpoint(context);
        }
    }
}
