using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class DataModel
    {
        public DataModel()
        {
            Columns = new Dictionary<int, ColumnModel>();
            Rows = new Dictionary<int, RowModel>();
        }

        /// <summary>
        /// The columns with ordinal position
        /// </summary>
        public Dictionary<int,ColumnModel> Columns { get; set; }

        /// <summary>
        /// The row data with columns and values
        /// </summary>
        public Dictionary<int,RowModel> Rows { get; set; }

        /// <summary>
        /// The table name to import the data into
        /// </summary>
        public string TableName { get; set; }

        public RowModel this[int r]
        {
            get
            {
                if(Rows.ContainsKey(r))
                { return Rows[r]; }
                return null;
            }
            set
            {
                if(Rows.ContainsKey(r))
                { Rows[r] = value; }
                else
                { Rows.Add(r, value); }
            }
        }

    }
}
