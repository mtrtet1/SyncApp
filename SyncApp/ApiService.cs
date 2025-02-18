using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SyncApp
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ApiService(string apiBaseUrl, string apiToken)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
            _apiBaseUrl = apiBaseUrl;
        }

        public void SyncCategory(Dictionary<string, object> category)
        {
            var json = JsonSerializer.Serialize(category);
            SendToApi("/categories", json);
        }

        public void SyncProduct(Dictionary<string, object> product)
        {
            var json = JsonSerializer.Serialize(product);
            SendToApi("/products", json);
        }

        public void SyncOrder(Dictionary<string, object> order)
        {
            var json = JsonSerializer.Serialize(order);
            SendToApi("/orders", json);
        }

        private void SendToApi(string endpoint, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.PostAsync(_apiBaseUrl + endpoint, content);
        }
    }
}
