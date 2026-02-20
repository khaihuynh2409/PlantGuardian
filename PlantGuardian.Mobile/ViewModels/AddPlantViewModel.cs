using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;

namespace PlantGuardian.Mobile.ViewModels;

public partial class AddPlantViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    string name = string.Empty;

    [ObservableProperty]
    string species = string.Empty;

    [ObservableProperty]
    string selectedPlantType = "Other";

    [ObservableProperty]
    string soilType = string.Empty;

    [ObservableProperty]
    DateTime datePlanted = DateTime.Today;

    [ObservableProperty]
    string statusMessage = string.Empty;

    public List<string> PlantTypes { get; } = new()
    {
        "BlackBean",
        "Soybean",
        "FavaBean",
        "Succulent",
        "Cactus",
        "Other"
    };

    public AddPlantViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    async Task SavePlant()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            StatusMessage = "Vui lòng nhập tên cây";
            return;
        }

        StatusMessage = "Đang lưu...";

        var newPlant = new Plant
        {
            Name = Name,
            Species = string.IsNullOrWhiteSpace(Species) ? "Unknown" : Species,
            PlantType = SelectedPlantType,
            SoilType = string.IsNullOrWhiteSpace(SoilType) ? "Standard" : SoilType,
            DatePlanted = DatePlanted,
            ImageUrl = "plant_placeholder.png" // Default image for now
        };

        var response = await _apiService.PostAsync<Plant, Plant>("plants", newPlant);

        if (response != null)
        {
            StatusMessage = "Thành công!";
            await App.Current!.MainPage!.DisplayAlert("Thành công", "Đã thêm cây mới", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            StatusMessage = "Lỗi khi lưu cây. Vui lòng thử lại.";
        }
    }
}
