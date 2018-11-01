using AzureDataManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureDataManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace AzureDataManagement.DataProvider
{
    public class SQLProvider : IDataProvider
    {

        #region Convert to DataModel

        private DataModel GetDataModel(SqlDataReader reader)
        {
            var ret = new DataModel();
            var counter = 0;

            ret.Columns = GetColumnData(reader);
            while(reader.Read())
            {
                ret.Rows[counter] = GetRowData(reader, ret.Columns, counter);
                counter++;
            }
            
            return ret;
        }

        private RowModel GetRowData(SqlDataReader reader, Dictionary<int, ColumnModel> columns, int rowNumber)
        {
            var ret = new RowModel();
            ret.RowNumber = rowNumber;
            foreach(var col in columns.Values)
            {
                var rowVal = new RowValueModel();
                rowVal.ColumnName = col.Name;
                rowVal.ColumnValue = reader[col.Name].ToString();
                ret[col.Name] = rowVal;
            }
            return ret;
        }

        private Dictionary<int, ColumnModel> GetColumnData(SqlDataReader reader)
        {
            var ret = new Dictionary<int, ColumnModel>();
            int counter = 0;
            while(reader.FieldCount > counter)
            {
                var col = new ColumnModel();
                col.ColumnType = ConvertType(reader.GetFieldType(counter));
                col.Position = counter;
                col.Name = reader.GetName(counter);
                ret.Add(counter, col);
                counter++;
            }
            return ret;
        }

        private SqlDbType ConvertType(Type t)
        {
            if (t == typeof(int))
                return SqlDbType.Int;
            if (t == typeof(decimal))
                return SqlDbType.Decimal;
            if (t == typeof(DateTime))
                return SqlDbType.DateTime;

            return SqlDbType.NVarChar;

        }

        #endregion // Convert To DataModel

        /// <summary>
        /// Provides data from sql server into the data model format.
        /// </summary>
        public DataModel GetData(IProviderSettings settings)
        {
            DataModel ret = null;
            var sqlSettings = settings.GetSettings<SQLSettingsModel>();

            if(sqlSettings == null)
                throw new InvalidOperationException("The settings for this provider were not supplied.");

            using (var conn = new SqlConnection(sqlSettings.ConnectionString))
            {
                conn.Open();
                using (var command = new SqlCommand(sqlSettings.CommandText, conn))
                {
                    command.CommandType = sqlSettings.SqlCommandType;
                    command.CommandTimeout = sqlSettings.CommandTimeout;
                    var reader = command.ExecuteReader();
                    ret = GetDataModel(reader);
                }
                conn.Close();
            }

            return ret;
        }
    }
}
