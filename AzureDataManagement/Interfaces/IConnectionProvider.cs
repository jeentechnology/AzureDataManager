using AzureDataManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Interfaces
{
    public interface IConnectionProvider
    {
        bool UploadData(DataModel data, IProviderSettings settings);
    }
}
