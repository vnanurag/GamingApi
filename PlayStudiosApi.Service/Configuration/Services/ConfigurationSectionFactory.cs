using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace PlayStudiosApi.Service.Configuration.Services
{
    public class ConfigurationSectionFactory<TSection> : IConfigurationSectionFactory<TSection>
        where TSection : ConfigurationSection
    {
        public TSection Load(string sectionName)
        {
            return (TSection)ConfigurationManager.GetSection(sectionName);
        }
    }
}
