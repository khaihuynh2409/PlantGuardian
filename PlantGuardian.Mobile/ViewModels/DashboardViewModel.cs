using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using System.Collections.ObjectModel;

namespace PlantGuardian.Mobile.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    string weatherSummary;

    [ObservableProperty]
    string temperature;

    [ObservableProperty]
    ObservableCollection<string> alerts;

    public DashboardViewModel(ApiService apiService)
    {
        _apiService = apiService;
        Alerts = new ObservableCollection<string>();
        WeatherSummary = "Loading weather...";
    }

    [RelayCommand]
    async Task LoadData()
    {
        // Hardcoded generic location for demo (e.g., Ho Chi Minh City)
        double lat = 10.8231;
        double lon = 106.6297;

        var weather = await _apiService.GetAsync<PlantGuardian.Mobile.Models.WeatherResponse>($"weather?lat={lat}&lon={lon}");
        if (weather != null)
        {
            Temperature = $"{weather.Main.Temp}°C";
            WeatherSummary = $"Humidity: {weather.Main.Humidity}%";
        }
        else 
        {
            WeatherSummary = "Weather unavailable (API Key?)";
        }

        var weatherAlerts = await _apiService.GetAsync<List<string>>($"weather/alerts?lat={lat}&lon={lon}");
        if (weatherAlerts != null)
        {
            Alerts.Clear();
            foreach(var alert in weatherAlerts)
                Alerts.Add(alert);
        }
    }
}
