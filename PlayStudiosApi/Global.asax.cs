using Autofac;
using Autofac.Integration.WebApi;
using PlayStudiosApi.Autofac;
using PlayStudiosApi.Services.Autofac;
using Serilog;
using System.Configuration;
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

            // Register Modules
            builder
                .RegisterModule<ApiModule>();

            builder
                .RegisterModule<ServiceModule>();

            builder
                .Register(x =>
                {
                    var logger =  new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Seq(
                            serverUrl: ConfigurationManager.AppSettings["SeqUrl"])
                        .CreateLogger();

                    logger.Information("Starting Up...");

                    return logger;
                })
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
