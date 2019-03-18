using Autofac;
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

            //builder
            //    .RegisterGeneric(typeof(ConfigurationSectionFactory<>))
            //    .As(typeof(IConfigurationSectionFactory<>));

            // Register Services
            builder
                .RegisterType<QuestService>()
                .As<IQuestService>();

            //builder
            //    .RegisterType<QuestConfiguration>()
            //    .AsSelf();

            //builder
            //    .Register(x =>
            //    {
            //        var factory = x.Resolve<IConfigurationSectionFactory<QuestConfiguration>>();
            //        var section = factory.Load("QuestConfig");
            //        return section;
            //    })
            //    .As<QuestConfiguration>();
        }
    }
}
