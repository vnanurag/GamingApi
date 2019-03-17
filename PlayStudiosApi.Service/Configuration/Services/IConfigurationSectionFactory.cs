using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace PlayStudiosApi.Service.Configuration.Services
{
    /// <summary>
    /// Interface for loading configuration sections
    /// </summary>
    /// <typeparam name="TSection">The type of configuration section to load</typeparam>
    public interface IConfigurationSectionFactory<TSection>
        where TSection: ConfigurationSection
    {
        /// <summary>
        /// Load the specified configuration section
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        TSection Load(string sectionName);
    }
}
