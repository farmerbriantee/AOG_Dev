using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using AOG.Classes;
using System.Collections.Generic;

namespace AOG
{
    public class AgShareClient
    {
        private HttpClient client;
        private string baseUrl;
        private string apiKey;
        private readonly FormGPS gps;

        private void RebuildClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);
        }


        public AgShareClient(string serverUrl, string key)
        {
            baseUrl = serverUrl.TrimEnd('/');   
            apiKey = key;

            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);
        }

        public void SetApiKey(string key)
        {
            apiKey = key;
            RebuildClient();
        }

        public void SetBaseUrl(string url)
        {
            baseUrl = url.TrimEnd('/');
            RebuildClient();
        }

        public async Task<(bool ok, string message)> CheckApiAsync()
        {
            try
            {
                using (var tempClient = new HttpClient())
                {
                    tempClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    tempClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);

                    string requestUrl = $"{baseUrl}/api/fields";

                    //Debug.WriteLine("----- API CHECK START -----");
                    //Debug.WriteLine("Request URL: " + requestUrl);
                    //Debug.WriteLine("Authorization: ApiKey " + apiKey);
                    //Debug.WriteLine("Accept: application/json");

                    var response = await tempClient.GetAsync(requestUrl);

                    //Debug.WriteLine("StatusCode: " + (int)response.StatusCode);
                    //Debug.WriteLine("ReasonPhrase: " + response.ReasonPhrase);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine("Response body: " + responseBody);
                    //Debug.WriteLine("----- API CHECK END -----");

                    if (response.IsSuccessStatusCode)
                        return (true, "Connection OK");
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        return (false, "Invalid API key");
                    else
                        return (false, $"Status {response.StatusCode}: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<(bool ok, string message)> UploadFieldAsync(Guid fieldId, object fieldPayload)
        {
            try
            {
                var json = JsonConvert.SerializeObject(fieldPayload, Formatting.Indented);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{baseUrl}/api/fields/{fieldId}", content);
                var error = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return (true, "Upload successful");
                else
                    return (false, $"Upload failed: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return (false, $"Exception: {ex.Message}");
            }
        }
        public async Task<List<AgShareGetOwnFieldDto>> GetOwnFieldsAsync()
        {
            var url = $"{baseUrl}/api/fields/";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AgShareGetOwnFieldDto>>(json);
        }


        public async Task<string> DownloadFieldAsync(Guid fieldId)
        {
            var response = await client.GetAsync($"{baseUrl}/api/fields/{fieldId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPublicFieldsAsync(double lat, double lon, double radius = 50)
        {
            var url = $"{baseUrl}/api/fields/public?lat={lat}&lon={lon}&radius={radius}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
