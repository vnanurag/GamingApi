using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayStudiosApi.DataAccess.Autofac
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register DataAccess Module
            builder
                .RegisterModule<DataAccessModule>();
        }
    }
}
