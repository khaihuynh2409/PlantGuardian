using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantGuardian.API.Services;

namespace PlantGuardian.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<ActionResult<WeatherResponse>> GetWeather([FromQuery] double lat, [FromQuery] double lon)
        {
            var weather = await _weatherService.GetWeatherAsync(lat, lon);
            if (weather == null) return BadRequest("Could not fetch weather data. Check API Key or coordinates.");

            return Ok(weather);
        }

        [HttpGet("alerts")]
        public async Task<ActionResult<List<string>>> GetWeatherAlerts([FromQuery] double lat, [FromQuery] double lon)
        {
            var weather = await _weatherService.GetWeatherAsync(lat, lon);
            if (weather == null) return BadRequest("Could not fetch weather data.");

            var alerts = new List<string>();

            // Logic from user requirement: Humidity > 85% or Rain
            if (weather.Main.Humidity > 85)
            {
                alerts.Add("High humidity detected (>85%). Avoid watering succulents today.");
            }

            if (weather.Weather.Any(w => w.Main.Contains("Rain", StringComparison.OrdinalIgnoreCase) || 
                                         w.Description.Contains("rain", StringComparison.OrdinalIgnoreCase)))
            {
                alerts.Add("Rain is in the forecast. Check if your plants are in a sheltered location.");
            }

            if (weather.Main.Temp < 10)
            {
                alerts.Add("Low temperature warning (<10°C). Bring sensitive plants inside.");
            }
            else if (weather.Main.Temp > 35)
            {
                 alerts.Add("High temperature warning (>35°C). Ensure plants have shade.");
            }

            return Ok(alerts);
        }
    }
}
