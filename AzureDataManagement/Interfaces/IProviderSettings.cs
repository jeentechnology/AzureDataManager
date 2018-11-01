using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Interfaces
{
    public interface IProviderSettings
    {
        t GetSettings<t>() where t : class;

        void RegisterSettings<t>(t SettingsData) where t : class;
    }
}
