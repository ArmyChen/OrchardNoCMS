﻿using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using System;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using System.Collections.Generic;
using OrchardVNext.Middleware;

namespace OrchardVNext.Routing {
    public class OrchardRouterMiddleware {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public OrchardRouterMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider) {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext httpContext) {
            Console.WriteLine("Begin Routing Request");

            var router = httpContext.RequestServices.GetService<IRouteBuilder>().Build();

            var context = new RouteContext(httpContext);
            context.RouteData.Routers.Add(router);

            await router.RouteAsync(context);

            if (!context.IsHandled) {
                await _next.Invoke(httpContext);
            }

            Console.WriteLine("End Routing Request");
        }
    }
}