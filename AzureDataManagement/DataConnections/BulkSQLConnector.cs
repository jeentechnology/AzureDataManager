using AzureDataManagement.Interfaces;
using AzureDataManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.DataProvider
{
    public class BulkSQLConnector : IConnectionProvider
    {
        #region Private Members (ex naming: _fieldName)

        private string connection;
        
        #endregion //Private Members


        #region Properties (ex naming: private _PropertyName, public PropertyName)

        #endregion //Properties


        #region Constructors
        
        public BulkSQLConnector() { }

        #endregion //Constructors


        #region Private Methods (ex naming: MethodName())

        private int GetColumnLength(ColumnModel column)
        {
            if(column.MaxSize <= 250)
                return 250;
            if (column.MaxSize <= 500)
                return 500;
            if (column.MaxSize <= 1000)
                return 1000;
            if (column.MaxSize <= 2000)
                return 2000;

            return 4000;
        }

        /// <summary>
        /// Builds code to create a table in order to hold the uploaded data.
        /// </summary>
        /// <param name="data">The data model to be uploaded</param>
        /// <returns>the sql string to create the table.</returns>
        private string BuildCreateTableSql(DataModel data)
        {
            var first = true;
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'U')) DROP TABLE [dbo].[{0}]", data.TableName));
            sb.AppendLine(string.Format("CREATE TABLE [dbo].[{0}](", data.TableName));
            foreach (var dc in data.Columns.Values)
            {
                var dataTypeString = string.Empty;
                switch (dc.ColumnType)
                {
                    case SqlDbType.Decimal:
                    case SqlDbType.Float:
                        dataTypeString = "[decimal](21, 7) NULL";
                        break;
                    case SqlDbType.TinyInt:
                        dataTypeString = "[TINYINT] NULL";
                        break;
                    case SqlDbType.SmallInt:
                        dataTypeString = "[SMALLINT] NULL";
                        break;
                    case SqlDbType.Int:
                        dataTypeString = "[INT] NULL";
                        break;
                    case SqlDbType.BigInt:
                        dataTypeString = "[BIGINT] NULL";
                        break;
                    case SqlDbType.Bit:
                        dataTypeString = "[BIT] NULL";
                        break;
                    case SqlDbType.DateTime:
                        dataTypeString = "[DATETIME] NULL";
                        break;
                    case SqlDbType.NVarChar:
                        dataTypeString = string.Format("[NVARCHAR]({0})",GetColumnLength(dc));
                        break;
                    case SqlDbType.VarChar:
                        dataTypeString = string.Format("[VARCHAR]({0})", GetColumnLength(dc));
                        break;
                    default:
                        dataTypeString = string.Format("[VARCHAR]({0})", GetColumnLength(dc));
                        break;
                }
                if (first)
                {
                    sb.AppendLine(string.Format("[{0}] {1} ", dc.Name, dataTypeString));
                    first = false;
                }
                else
                {
                    sb.AppendLine(string.Format(", [{0}] {1} ", dc.Name, dataTypeString));
                }
            }
            sb.AppendLine(")");
            return sb.ToString();

        }

        private DataTable BuildDataTable(DataModel modelData)
        {
            var data = CreateDataTableStructure(modelData);
            foreach(var item in modelData.Rows.Values)
            {
                var dr = data.NewRow();
                foreach(var rowValue in item.RowData.Values)
                {
                    if(string.IsNullOrEmpty(rowValue.ColumnValue))
                    {
                        dr[rowValue.ColumnName] = null;
                    }
                    else
                    {
                        dr[rowValue.ColumnName] = rowValue.ColumnValue;
                    }
                }
                data.Rows.Add(dr);
            }
            return data;
        }

        private DataTable CreateDataTableStructure(DataModel data)
        {
            var ret = new DataTable(data.TableName) { MinimumCapacity = data.Rows.Values.Count };
            foreach (var c in data.Columns.Values)
            {
                switch (c.ColumnType)
                {
                    case SqlDbType.VarChar:
                    case SqlDbType.NVarChar:
                        ret.Columns.Add(c.Name, typeof(string));
                        break;
                    case SqlDbType.Int:
                    case SqlDbType.BigInt:
                        ret.Columns.Add(c.Name, typeof(int));
                        break;
                    case SqlDbType.Decimal:
                    case SqlDbType.Float:
                        ret.Columns.Add(c.Name, typeof(decimal));
                        break;
                    default:
                        ret.Columns.Add(c.Name, typeof(string));
                        break;
                }
            }
            return ret;

        }

        #endregion //Private Methods


        #region Public Methods (ex naming: MethodName())

        public bool UploadData(DataModel data, IProviderSettings settings)
        {
            var settingsData = settings.GetSettings<SQLConnectorModel>();

            if(settingsData == null)
                throw new InvalidOperationException("The settings for this provider were not supplied.");

            this.connection = settingsData.ConnectionString;

            var ret = true;
            var createTableSql = BuildCreateTableSql(data);
            var insertTable = BuildDataTable(data);
            try
            {
                using (var conn = new SqlConnection(connection))
                {
                    conn.Open();
                    
                    // Creates the table in the azure db
                    using (var command = new SqlCommand(createTableSql, conn))
                    {
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }

                    // Bulk Uplaod the data
                    using (var sbc = new SqlBulkCopy(conn))
                    {
                        sbc.BulkCopyTimeout = 100000;
                        sbc.DestinationTableName = string.Format("[dbo].[{0}]",  settingsData.UseDataModelForDestination ? data.TableName : settingsData.Destination);
                        sbc.EnableStreaming = true;
                        sbc.WriteToServer(insertTable);
                    }
                }
            }
            catch(Exception e)
            {
                ret = false;
            }


            return ret;
        }


        #endregion //Public Methods
    }
}
