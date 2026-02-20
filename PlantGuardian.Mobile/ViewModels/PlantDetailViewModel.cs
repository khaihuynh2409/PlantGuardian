using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;
using PlantGuardian.Mobile.Views;

namespace PlantGuardian.Mobile.ViewModels;

[QueryProperty(nameof(Plant), "Plant")]
public partial class PlantDetailViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    Plant plant = new();

    // True if this plant is one of the 3 bean types
    public bool IsBean => Plant?.IsBean ?? false;

    public PlantDetailViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    partial void OnPlantChanged(Plant value)
    {
        OnPropertyChanged(nameof(IsBean));
    }

    [RelayCommand]
    async Task DeletePlant()
    {
        bool confirm = await App.Current!.MainPage!.DisplayAlert("Xác nhận", "Bạn có chắc muốn xóa cây này?", "Xóa", "Hủy");
        if (!confirm) return;

        // await _apiService.DeleteAsync($"plants/{Plant.Id}");
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>Navigate to the specialized BeanDetailPage for bean plants.</summary>
    [RelayCommand]
    async Task ViewBeanDetails()
    {
        if (Plant == null) return;
        await Shell.Current.GoToAsync(nameof(BeanDetailPage), new Dictionary<string, object>
        {
            { "Plant", Plant }
        });
    }
}
