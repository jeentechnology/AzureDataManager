using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class RowValueModel
    {
        #region ColumnName -- string

        private string _columnName;
        public string ColumnName
        {
            get { return _columnName; }
            set
            {
                if (!Equals(_columnName, value))
                {
                    _columnName = value;
                }
            }
        }

        #endregion //ColumnName Region

        #region ColumnValue -- string

        private string _columnValue;
        public string ColumnValue
        {
            get { return _columnValue; }
            set
            {
                if (!Equals(_columnValue, value))
                {
                    _columnValue = value;
                }
            }
        }

        #endregion //ColumnValue Region
    }
}
