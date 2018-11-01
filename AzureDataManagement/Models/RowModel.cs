using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class RowModel
    {

        public RowModel()
        {
            RowData = new Dictionary<string, RowValueModel>();
        }

        public Dictionary<string,RowValueModel> RowData { get; set; }

        public int RowNumber { get; set; }

        public RowValueModel this[string c]
        {
            get
            {
                if(RowData.ContainsKey(c))
                { return RowData[c]; }
                return null;
            }
            set
            {
                if(RowData.ContainsKey(c))
                { RowData[c] = value; }
                else
                { RowData.Add(c, value); }
            }
        }
    }
}
