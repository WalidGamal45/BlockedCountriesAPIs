using BDAssignment.Application.Interfaces;
using BDAssignment.Application.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace BDAssignment.Application.Services
{
    public class GeoLookupService : IGeoLookupService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeoLookupService> _logger;

        public GeoLookupService(HttpClient httpClient, ILogger<GeoLookupService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IpLookupResultDto> GetCountryByIPAsync(string ipAddress)
        {
            try
            {
                string apiUrl = $"https://ipapi.co/{ipAddress}/json/";

                //  أضف الـ Header
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; MyApp/1.0)");

                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch IP info for {ipAddress} (Status: {response.StatusCode})");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IpLookupResultDto>(json);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching IP info");
                return null;
            }
        }

    }
}
