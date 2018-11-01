using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureDataManagement.Interfaces;
using AzureDataManagement.Models;
using System.IO;

namespace AzureDataManagement.DataConnections
{
    public class CSVConnector : IConnectionProvider
    {
        #region Internal Class Constants

        private const int flushFrequency = 1000000;

        #endregion // Internal Class Constants


        #region Shared Methods

        private string ConvertRowToCSVRow(RowModel row, Dictionary<int, ColumnModel> columns, bool isHeader)
        {
            var builder = new StringBuilder();
            foreach (var column in columns.OrderBy(c => c.Key))
            {
                if (isHeader)
                {
                    builder.Append($@"{column.Value.Name},");
                }
                else
                {
                    var rowValue = row[column.Value.Name];
                    var appendValue = rowValue == null ? string.Empty : rowValue.ColumnValue;

                    if ((column.Value.ColumnType == System.Data.SqlDbType.VarChar || column.Value.ColumnType == System.Data.SqlDbType.NVarChar)
                        && appendValue != string.Empty)
                        builder.Append($"\"{appendValue}\",");
                    else
                        builder.Append($@"{appendValue},");
                }
            }

            var rowString = builder.ToString().Substring(0, builder.Length - 1);
            return rowString;
        }

        #endregion // Shared Methods


        #region Stream File Methods

        /// <summary>
        /// Streams the data to a file periodically flushing the stream to keep the memory usage down.
        /// </summary>
        private bool StreamToCSVOutput(DataModel data, string filePath)
        {
            try
            {
                using (StreamWriter fileOuput = new StreamWriter(filePath))
                {
                    fileOuput.WriteLine(ConvertRowToCSVRow(null, data.Columns, true));
                    foreach (var row in data.Rows)
                    {
                        fileOuput.WriteLine(ConvertRowToCSVRow(row.Value, data.Columns, false));
                        if (CheckForFlush(row.Key))
                            fileOuput.Flush();
                    }
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether or not the stream needs to be flushed.
        /// </summary>
        private bool CheckForFlush(int RowNumber)
        {
            if (RowNumber % flushFrequency == 0)
                return true;
            return false;
        }

        #endregion // Stream File Methods


        #region Create Standard CSV Output
        private bool CreateCSVOutput(DataModel data, string filePath)
        {
            var builder = new StringBuilder();
            var directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);

            try
            {
                builder.AppendLine(ConvertRowToCSVRow(null, data.Columns, true));
                foreach(var row in data.Rows)
                {
                    builder.AppendLine(ConvertRowToCSVRow(row.Value, data.Columns, false));
                }
                File.WriteAllText(filePath, builder.ToString());
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        #endregion // Create Standard CSV Outpu


        #region IConnectionProvider
        public bool UploadData(DataModel data, IProviderSettings settings)
        {
            var settingsData = settings.GetSettings<CSVConnectorSettings>();

            if (settingsData == null)
                throw new InvalidOperationException("The settings for this provider were not supplied.");

            if (data.Rows.Count > flushFrequency)
                return StreamToCSVOutput(data, settingsData.FilePath);

            return CreateCSVOutput(data, settingsData.FilePath);
        }

        #endregion // IConnectionProvider
    }
}
