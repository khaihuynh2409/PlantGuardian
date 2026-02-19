using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;

namespace PlantGuardian.Mobile.ViewModels;

[QueryProperty(nameof(Plant), "Plant")]
public partial class PlantDetailViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    Plant plant;

    public PlantDetailViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    async Task DeletePlant()
    {
        bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", "Delete this plant?", "Yes", "No");
        if (!confirm) return;

        // Implementation of DELETE request would go here
        // await _apiService.DeleteAsync($"plants/{Plant.Id}");
        
        await Shell.Current.GoToAsync("..");
    }
}
