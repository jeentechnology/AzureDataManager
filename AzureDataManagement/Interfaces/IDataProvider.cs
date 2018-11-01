using AzureDataManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Interfaces
{
    public interface IDataProvider
    {
        DataModel GetData(IProviderSettings settings);
    }
}
