using AzureDataManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Interfaces
{
    public interface IApiConnector<T>
    {
        Task<ApiResult> SendData(T payload, ApiSettings config);

        Task<List<ApiResult>> SendData(List<T> payload, int intervalDelay, ApiSettings config);

        Task<ApiResult> GetData(ApiSettings config);
 
    }
}
