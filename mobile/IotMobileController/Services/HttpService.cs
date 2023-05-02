using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IotMobileController.Services
{
    public class HttpService
    {
       
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _serializerOptions;

        public HttpService()
        {
            
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }


        public async Task<dynamic> GetDataAsync()
        {
            Uri uri = new("http://100.88.95.100:5000/leds");
            try
            {
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic dynJson = JsonConvert.DeserializeObject<dynamic>(content);
                    return dynJson;
                }
            } catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
            }
            return new List<float>();
        }

        

    }
}
