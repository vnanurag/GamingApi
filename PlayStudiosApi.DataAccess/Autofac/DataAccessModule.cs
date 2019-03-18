using Autofac;
using PlayStudiosApi.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStudiosApi.DataAccess.Autofac
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register Services
            builder
                .RegisterType<QuestRepository>()
                .As<IQuestRepository>();
        }
    }
}
