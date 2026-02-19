using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;
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
    async Task AddPlant()
    {
        // Navigate to Add Page (Not implemented yet, just a stub)
        // await App.Current.MainPage.DisplayAlert("Coming Soon", "Add Plant Feature", "OK");

        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);
                    await sourceStream.CopyToAsync(localFileStream);

                    await App.Current.MainPage.DisplayAlert("Success", $"Photo saved to {localFilePath}", "OK");
                    
                    // Here you would upload the photo to the backend
                    // var uploadedUrl = await _apiService.UploadImage(localFilePath);
                    // var newPlant = new Plant { ImageUrl = uploadedUrl, ... };
                    // await _apiService.PostAsync("plants", newPlant);
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Camera not supported", "OK");
            }
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Error", $"Camera failed: {ex.Message}", "OK");
        }
    }
}
