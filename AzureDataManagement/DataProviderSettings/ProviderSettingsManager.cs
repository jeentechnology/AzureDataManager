using AzureDataManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.DataProviderSettings
{
    public class ProviderSettingsManager : IProviderSettings
    {

        private Dictionary<Type, object> settingsRepository = new Dictionary<Type, object>();

        public t GetSettings<t>() where t : class
        {
            if (settingsRepository.ContainsKey(typeof(t)))
                return (t)settingsRepository[typeof(t)];

            return null;
        }

        public void RegisterSettings<t>(t SettingsData) where t : class
        {
            var key = SettingsData.GetType();

            if (settingsRepository.ContainsKey(key))
                settingsRepository[key] = SettingsData;
            else
                settingsRepository.Add(key, SettingsData);
        }
    }
}
