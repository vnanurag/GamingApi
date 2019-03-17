using Autofac;
using PlayStudiosApi.Service.Configuration;
using PlayStudiosApi.Service.Services;
using PlayStudiosApi.Service.Services.Interfaces;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using PlayStudiosApi.Service.Configuration.Services;

namespace PlayStudiosApi.Service.Autofac
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterGeneric(typeof(ConfigurationSectionFactory<>))
                .As(typeof(IConfigurationSectionFactory<>));

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
