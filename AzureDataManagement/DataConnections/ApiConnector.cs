using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureDataManagement.Interfaces;
using AzureDataManagement.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading;

namespace AzureDataManagement.DataConnections
{
    public class ApiConnector<T> : IApiConnector<T> where T : class
    {
        public async Task<ApiResult> SendData(T payload, ApiSettings config)
        {
            var ret = new ApiResult();
            using (var client = new HttpClient())
            {
                try
                {
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    ret = await MakeRequest(config, client, payload);
                }
                catch(Exception e)
                {
                    // add logging for errors
                    ret.Response = e.ToString();
                    ret.Success = false;
                }

            }
            return ret;
        }

        public async Task<List<ApiResult>> SendData(List<T> payload, int intervalDelay, ApiSettings config)
        {
            var ret = new List<ApiResult>();
            var first = true;
            using (var client = new HttpClient())
            {
                foreach (var item in payload)
                {
                    if (first) first = false;
                    else Thread.Sleep(intervalDelay);
                    try
                    {
                        var res = await MakeRequest(config, client, item);
                        ret.Add(res);
                    }
                    catch(Exception e)
                    {
                        // add logging for errors
                        Console.WriteLine(e.ToString());
                        
                    }
                }
            }
            return ret;
        }

        public async Task<ApiResult> GetData(ApiSettings config)
        {
            var ret = new List<ApiResult>();
            using (var client = new HttpClient())
            {
                try
                {
                    var res = await MakeRequest(config, client, null);
                    ret.Add(res);
                }
                catch (Exception e)
                {
                    // add logging for errors
                    Console.WriteLine(e.ToString());

                }
            }
            return ret.FirstOrDefault();
        }

        private async Task<ApiResult> MakeRequest(ApiSettings config, HttpClient client, T payload)
        {
            var ret = new ApiResult();
            var json = payload == null ? string.Empty : JsonConvert.SerializeObject(payload);
            var request = BuildRequest(config, json);
            
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                ret.Response = await response.Content.ReadAsStringAsync();
                ret.Success = true;
            }
            else
            {
                ret.Success = false;
                ret.Response = response.ToString();
            }
            return ret;
        }

        private HttpRequestMessage BuildRequest(ApiSettings conifg, string json)
        {
            var ret = new HttpRequestMessage();
            ret.RequestUri = new Uri(conifg.Url);
            if(!string.IsNullOrEmpty(json))
                ret.Content = new StringContent(json, Encoding.UTF8, "application/json");
            ret.Method = conifg.RequestType;
            if(conifg.Headers.Count > 0)
                conifg.Headers.ForEach(h => ret.Headers.Add(h.Key, h.Value));

            // // TODO: will need to work on supporting auth types better
            return ret;
        }

    }
}
