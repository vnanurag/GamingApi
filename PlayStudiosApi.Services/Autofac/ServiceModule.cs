using Autofac;
using PlayStudiosApi.DataAccess.Autofac;
using PlayStudiosApi.Services.Configuration;
using PlayStudiosApi.Services.Services;
using PlayStudiosApi.Services.Services.Interfaces;

namespace PlayStudiosApi.Services.Autofac
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register Services
            builder
                .RegisterType<QuestService>()
                .As<IQuestService>();

            // Register modules
            builder
                .RegisterModule<DataAccessModule>();
        }
    }
}
