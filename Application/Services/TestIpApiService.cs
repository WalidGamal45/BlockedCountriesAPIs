using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BDAssignment.Application.Services
{
    public class TestIpApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TestIpApiService> _logger;

        public TestIpApiService(HttpClient httpClient, ILogger<TestIpApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetIpInfoAsync(string ipAddress)
        {
            string url = $"https://ipapi.co/{ipAddress}/json/";

            //  أضف الـ Header
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; TestClient/1.0)");

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                _logger.LogInformation(data);
                return data;
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error: {response.StatusCode} - {error}");
                return $"Error: {response.StatusCode} - {error}";
            }
        }

    }
}
