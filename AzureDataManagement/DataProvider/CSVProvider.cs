using AzureDataManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureDataManagement.Utilities;
using AzureDataManagement.Interfaces;
using LumenWorks.Framework.IO.Csv;

namespace AzureDataManagement.DataProvider
{
    public class CSVProvider : IDataProvider
    {

        #region Private Members (ex naming: _fieldName)

        private string filePath;
        private char delim;

        #endregion //Private Members

        #region Properties (ex naming: private _PropertyName, public PropertyName)

        #endregion //Properties

        #region Constructors

        public CSVProvider() { }

        #endregion //Constructors

        #region Private Methods (ex naming: MethodName())

        /// <summary>
        /// Parses out the columns from the first row of the file
        /// </summary>
        /// <param name="columnLine">the string representation of the first row in the file</param>
        /// <param name="delim">the character used to delim the file</param>
        /// <returns></returns>
        private Dictionary<int, ColumnModel> GetColumns(string columnLine, char delim)
        {
            var ret = new Dictionary<int, ColumnModel>();
            var position = 0;
            foreach(var item in columnLine.SplitQuotedList(delim))
            {
                ret.Add(position++, new ColumnModel
                {
                    Name = item,
                    Position = position,
                    ColumnType = System.Data.SqlDbType.VarChar
                });
            }
            return ret;
        }

        private Dictionary<int, ColumnModel> GetColumns(string[] columns)
        {
            var ret = new Dictionary<int, ColumnModel>();
            var position = 0;
            foreach (var item in columns)
            {
                ret.Add(position++, new ColumnModel
                {
                    Name = item,
                    Position = position,
                    ColumnType = System.Data.SqlDbType.VarChar
                });
            }
            return ret;
        }

        /// <summary>
        /// Parses the row values
        /// </summary>
        /// <param name="rowLine">the string representation of the row value</param>
        /// <param name="columns">the columns available for this file</param>
        /// <param name="delim">the delim character used in the file</param>
        /// <returns>a collection of row values</returns>
        private RowModel GetRow(string rowLine, DataModel data, char delim, int rowNum)
        {
            var ret = new RowModel();
            var position = 0;
            var rowItems = rowLine.SplitQuotedList(delim);

            if (data.Columns.Count != rowItems.Count)
                throw new InvalidDataException($"Could not parse data: RowValue: {rowLine}");

            foreach (var item in rowItems)
            {
                var rowVal = new RowValueModel();

                if(item.Length > data.Columns[position].MaxSize)
                {
                    data.Columns[position].MaxSize = item.Length;
                }

                rowVal.ColumnName = data.Columns[position++].Name;
                rowVal.ColumnValue = item;
                ret[rowVal.ColumnName] = rowVal;

                if(position >= data.Columns.Keys.Count)
                {
                    position = 0;
                }
            }
            ret.RowNumber = rowNum;
            return ret;
        }

        private RowModel GetRow(CsvReader rowLine, DataModel data, int rowNum)
        {
            var ret = new RowModel();

            for (int i = 0; i < data.Columns.Count; i++)
            {
                var rowval = new RowValueModel();
                rowval.ColumnName = data.Columns[i].Name;
                rowval.ColumnValue = rowLine[i].ToString();
                ret[rowval.ColumnName] = rowval;
            }
            ret.RowNumber = rowNum;
            return ret;
        }

        #endregion //Private Methods

        #region Public Methods (ex naming: MethodName())

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        public DataModel GetData(IProviderSettings settings)
        {
            var csvSettings = settings.GetSettings<CSVSettingsModel>();

            if (csvSettings == null)
                throw new InvalidOperationException("The settings for this provider were not supplied.");

            filePath = csvSettings.FilePath;
            delim = csvSettings.Delimeter;

            //var line = string.Empty;
            //var first = true;
            int position = 0;
            var file = new FileInfo(filePath);

            // Holds the data to be returned
            var ret = new DataModel();
            ret.TableName = Path.GetFileNameWithoutExtension(filePath);

            // read the text file and translate into data models
            using (CsvReader csv = new CsvReader(new StreamReader(filePath), true))
            {
                ret.Columns = GetColumns(csv.GetFieldHeaders());

                while(csv.ReadNextRecord())
                {
                    try
                    {
                        ret[position] = GetRow(csv, ret, position);
                        position++;
                    }
                    catch (Exception e)
                    {
                        var ex = e.Data;
                    }
                }
            }

            //// read the text file and translate into data models
            //using (StreamReader sr = new StreamReader(filePath))
            //{
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        try
            //        {
            //            if (first)
            //            {
            //                first = false;
            //                ret.Columns = GetColumns(line, delim);
            //            }
            //            else
            //            {
            //                ret[position] = GetRow(line, ret, delim, position);
            //                position++;
            //            }
            //        }
            //        catch(Exception e)
            //        {
            //            var ex = e.Data;
            //        }
            //    }
            //}
            return ret;
        }

        #endregion //Public Methods

    }
}
