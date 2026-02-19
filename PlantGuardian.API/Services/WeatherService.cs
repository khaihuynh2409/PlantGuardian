using Newtonsoft.Json;

namespace PlantGuardian.API.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherResponse?> GetWeatherAsync(double latitude, double longitude)
        {
            string apiKey = _configuration["OpenWeatherMap:ApiKey"]!;
            if (string.IsNullOrEmpty(apiKey)) return null;

            var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WeatherResponse>(content);
        }
    }
}
