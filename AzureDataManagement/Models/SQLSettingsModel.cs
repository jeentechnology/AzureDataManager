using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class SQLSettingsModel
    {

        public SQLSettingsModel()
        {
            SqlCommandType = CommandType.StoredProcedure;
        }

        public string ConnectionString { get; set; }

        public CommandType SqlCommandType { get; set; }

        public string CommandText { get; set; }

        public int CommandTimeout { get; set; }
    }
}
