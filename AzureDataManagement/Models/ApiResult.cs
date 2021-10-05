using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class ApiResult
    {
        public string Response { get; set; }
        public string CallBackUrl { get; set; }
        public bool Success { get; set; }
    }
}
