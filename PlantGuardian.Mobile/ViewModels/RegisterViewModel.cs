using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;

namespace PlantGuardian.Mobile.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    string username = string.Empty;

    [ObservableProperty]
    string email = string.Empty;

    [ObservableProperty]
    string password = string.Empty;

    [ObservableProperty]
    string statusMessage = string.Empty;

    public RegisterViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    async Task Register()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
        {
            StatusMessage = "Please fill all fields";
            return;
        }

        StatusMessage = "Registering...";
        // Note: The API returns the User object, not a token, on register.
        // Flow: Register -> Login or Auto-Login
        var response = await _apiService.PostAsync<RegisterModel, dynamic>("auth/register", new RegisterModel { Username = Username, Email = Email, Password = Password });

        if (response != null)
        {
            StatusMessage = "Success! Please login.";
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            StatusMessage = "Registration failed.";
        }
    }
}
