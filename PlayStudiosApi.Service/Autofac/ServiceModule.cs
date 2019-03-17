using Autofac;
using NLog;
using PlayStudiosApi.Service.Services;
using PlayStudiosApi.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayStudiosApi.Service.Autofac
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

            builder
                .RegisterType<Logger>()
                .As<ILogger>()
                .SingleInstance();
        }
    }
}
