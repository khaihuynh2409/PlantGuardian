using PlantGuardian.API.Models;

namespace PlantGuardian.API.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse?> GetWeatherAsync(double latitude, double longitude);
    }

    public class WeatherResponse
    {
        public MainInfo Main { get; set; } = new();
        public List<WeatherInfo> Weather { get; set; } = new();
    }

    public class MainInfo
    {
        public double Temp { get; set; }
        public double Humidity { get; set; }
    }

    public class WeatherInfo
    {
        public string Main { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
