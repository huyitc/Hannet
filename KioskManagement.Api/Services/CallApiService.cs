using KioskManagement.Model.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KioskManagement.Common.Ultilities
{
    public static class Lib
    {
        public static async Task<HttpResponseMessage> MethodPostAsync(string url, object data)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    string stringData = JsonConvert.SerializeObject(data);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(url, contentData).Result;
                    return response;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static async Task<IRestResponse> MethodPostAsyncHanet(string url, object data)
        {
            var client = new RestClient(url) { Timeout = 5000 };
            var request = new RestRequest { Method = Method.POST };
            request.AddParameter("token", AstecConstant.tokenHanet);
            request.AddObject(data);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        public static IRestResponse MethodPostNotAsync(string url, object data)
        {
            var dataJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            var client = new RestClient(url) { Timeout = 5000 };
            var request = new RestRequest { Method = Method.POST };
            //request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("text/plain", dataJson, ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        public static async Task<IRestResponse> MethodGetAsync(string url)
        {
            var client = new RestClient(url) { Timeout = 5000 };
            var request = new RestRequest { Method = Method.GET };
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("token", AstecConstant.tokenHanet);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        public static async Task<HttpResponseMessage> MethodPostFile(string url,EmployeeHanet emHan)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new StringContent(AstecConstant.tokenHanet), "token");
            form.Add(new StringContent(emHan.name), "name");
            form.Add(new StringContent(emHan.title), "title");
            form.Add(new StringContent(emHan.aliasID), "aliasID");
            form.Add(new StringContent(emHan.placeID), "placeID");
            form.Add(new ByteArrayContent(emHan.file, 0, emHan.file.Length), "file", "hello1.jpg");
            HttpResponseMessage response = await httpClient.PostAsync(url, form);

            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            return response;
        }

        public static async Task<IRestResponse> MethodPostUpdateEmployeeHanet(string url, object data)
        {
            var client = new RestClient(url) { Timeout = 5000 };
            var request = new RestRequest { Method = Method.POST };
            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("token", AstecConstant.tokenHanet);
            request.AddObject(data);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        public static async Task<IRestResponse> MethodPostWithParamAsync(string url, Dictionary<string, object> lstParameter)
        {
            var client = new RestClient(url) { Timeout = 5000 };
            var request = new RestRequest { Method = Method.POST };
            request.AddHeader("Content-Type", "text/plain");
            if (lstParameter != null && lstParameter.Count > 0)
            {
                foreach (var param in lstParameter)
                {
                    request.AddQueryParameter(param.Key, Common.ConvertToString(param.Value));
                }
            }
            var response = await client.ExecuteAsync(request);
            return response;
        }

        public static IRestResponse MethodPostWithParamNotAsync(string url, Dictionary<string, object> lstParameter)
        {
            var client = new RestClient(url) { Timeout = 5000 };
            var request = new RestRequest { Method = Method.POST };
            request.AddHeader("Content-Type", "text/plain");
            if (lstParameter != null && lstParameter.Count > 0)
            {
                foreach (var param in lstParameter)
                {
                    request.AddQueryParameter(param.Key, Common.ConvertToString(param.Value));
                }
            }
            var response = client.Execute(request);
            return response;
        }
    }
}
