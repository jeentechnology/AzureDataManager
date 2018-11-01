using AzureDataManagement.Interfaces;
using AzureDataManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AzureDataManagement
{
    public class DataManager
    {
        #region Properties

        private readonly IDataProvider provider;
        private readonly IConnectionProvider connection;
        private DataModel data;


        public bool HasData { get; set; }

        #endregion // Properties


        public DataManager(IDataProvider provider, IConnectionProvider connection)
        {
            this.provider = provider;
            this.connection = connection;
        }

        #region Internal Methods

        /// <summary>
        /// calls the provider and returns data
        /// </summary>
        private async Task<DataModel> GetDataModel(IProviderSettings settings)
        {
            return provider.GetData(settings);
        }

        #endregion // Internal Methods

        #region Public Methods
        
        public async Task<bool> UploadData(IProviderSettings settings)
        {
            return connection.UploadData(await GetDataModel(settings), settings);
        }

        #endregion // Public Methods
    }
}
