using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Models
{
    public class ApiSettings
    {
        public ApiSettings()
        {
            Headers = new List<KeyValuePair<string, string>>();
        }


        public string Url { get; set; }
        public AuthTypes Auth { get; set; }
        public string ClientKey { get; set; }
        public string ClientSecret { get; set;}
        public HttpMethod RequestType { get; set; }
        public List<KeyValuePair<string,string>> Headers { get; set; }
        
    }

    public enum AuthTypes
    {
        Header,
        Oauth
    }
}
