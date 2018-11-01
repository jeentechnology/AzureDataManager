using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class SQLConnectorModel
    {
        public SQLConnectorModel()
        {
            UseDataModelForDestination = false;
        }

        public string ConnectionString { get; set; }

        public string Destination { get; set; }

        public bool UseDataModelForDestination { get; set; }
    }
}
