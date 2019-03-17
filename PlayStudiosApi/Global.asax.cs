using Autofac;
using Autofac.Integration.WebApi;
using NLog;
using PlayStudiosApi.Autofac;
using PlayStudiosApi.Service.Autofac;
using PlayStudiosApi.Service.Services;
using PlayStudiosApi.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PlayStudiosApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitializeIoC();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void InitializeIoC()
        {
            // Create the container builder
            var builder = new ContainerBuilder();

            // Register Api controllers
            builder
                .RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register Modules
            builder
                .RegisterModule<ApiModule>();

            builder
                .RegisterModule<ServiceModule>();

            builder
                .RegisterType<Logger>()
                .As<ILogger>()
                .SingleInstance();


            // Build the container
            var container = builder.Build();

            // Configure the Web Api with the dependency resolver
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}
