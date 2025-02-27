using System.Text;
using System.Text.Json;

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
            _httpClient.DefaultRequestHeaders.Add("test", "true");
            _apiBaseUrl = apiBaseUrl;
        }


        public async void GetOrders()
        {
            var response = await GetFromApi("/orders/get-sync");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<AppResponse>(responseBody);
                if (res.success)
                {
                    MessageBox.Show(responseBody);
                }
                else
                {
                    MessageBox.Show(res.message);
                    Console.WriteLine(res.data);
                }
            }
            else
            {
                MessageBox.Show($"خطأ في التحديث : {response.ReasonPhrase}");
            }
        }
        public async void GetCategory()
        {
            var response = await GetFromApi("/main_categories");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<AppResponse>(responseBody);
                if (res.success)
                {
                    MessageBox.Show(responseBody);
                }
                else
                {
                    MessageBox.Show(res.message);
                    Console.WriteLine(res.data);
                }
            }
            else
            {
                MessageBox.Show($"خطأ في التحديث : {response.ReasonPhrase}");
            }
        }
        public async void SyncCategory(Dictionary<string, object> category)
        {
            var json = JsonSerializer.Serialize(category);
            await SendToApi("/main_categories", json);
        }

        public async Task<string> SyncProduct(Dictionary<string, object> product)
        {
            //var json = JsonSerializer.Serialize(product);
            var json = JsonSerializer.Serialize(product, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Match Laravel's expected field names
                WriteIndented = true // Optional: Pretty-print JSON for debugging
            });

            var response = await SendToApi("/products/add", json);
            string responseBody = await response.Content.ReadAsStringAsync();
            AppResponse? res = null;

            try
            {
                res = JsonSerializer.Deserialize<AppResponse>(responseBody);
            }
            catch (Exception ex)
            {

            }
            if (response.IsSuccessStatusCode)
            {
            }
            else
            {
                //MessageBox.Show($"خطأ في التحديث : {response.ReasonPhrase}");
            }


            return res != null ? res.message : "Deserialize Error Ex";  
        }

        public async void SyncOrder(Dictionary<string, object> order)
        {
            var json = JsonSerializer.Serialize(order);
            await SendToApi("/orders", json);
        }

        private async Task<HttpResponseMessage> SendToApi(string endpoint, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(_apiBaseUrl + "/api" + endpoint, content);
        }

        private async Task<HttpResponseMessage> GetFromApi(string endpoint)
        {
            return await _httpClient.GetAsync(_apiBaseUrl + "/api" + endpoint);
        }
    }



}
