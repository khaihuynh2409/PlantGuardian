using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;

namespace PlantGuardian.Mobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    string username;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    string statusMessage;

    public LoginViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Please enter username and password";
            return;
        }

        StatusMessage = "Logging in...";
        var response = await _apiService.PostAsync<LoginModel, string>("auth/login", new LoginModel { Username = Username, Password = Password });

        if (!string.IsNullOrEmpty(response))
        {
            StatusMessage = "Success!";
            _apiService.SetToken(response); // Basic token handling
            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        }
        else
        {
            StatusMessage = "Login failed. Check credentials.";
        }
    }
}
