using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using Module = Autofac.Module;

namespace PlayStudiosApi.Autofac
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register Api controllers
            builder
                .RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}