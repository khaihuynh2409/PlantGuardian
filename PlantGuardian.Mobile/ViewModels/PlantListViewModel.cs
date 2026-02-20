using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;
using PlantGuardian.Mobile.Views;
using System.Collections.ObjectModel;

namespace PlantGuardian.Mobile.ViewModels;

public partial class PlantListViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    ObservableCollection<Plant> plants;

    public PlantListViewModel(ApiService apiService)
    {
        _apiService = apiService;
        Plants = new ObservableCollection<Plant>();
    }

    [RelayCommand]
    async Task LoadPlants()
    {
        var plantList = await _apiService.GetAsync<List<Plant>>("plants");
        if (plantList != null)
        {
            Plants.Clear();
            foreach(var p in plantList)
            {
                // Placeholder image if empty
                if(string.IsNullOrEmpty(p.ImageUrl)) p.ImageUrl = "plant_placeholder.png"; 
                Plants.Add(p);
            }
        }
    }

    [RelayCommand]
    private async Task AddPlant()
    {
        await Shell.Current.GoToAsync(nameof(Views.AddPlantPage));
    }
}
