using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AzureDataManagement.Models
{
    public class ColumnModel
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public SqlDbType ColumnType { get; set; }
        public int MaxSize { get; set; }


        public override string ToString()
        {
            return string.Format("{0} - {1}", Position, Name);
        }
    }
}
