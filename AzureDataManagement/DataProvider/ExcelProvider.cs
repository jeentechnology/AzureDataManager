using AzureDataManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureDataManagement.Utilities;
using AzureDataManagement.Interfaces;

namespace AzureDataManagement.DataProvider
{
    public class ExcelProvider : IDataProvider
    {
        public DataModel GetData(IProviderSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
