using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;

namespace PlantGuardian.Mobile.ViewModels;

[QueryProperty(nameof(Plant), "Plant")]
public partial class BeanDiaryEntryViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    Plant plant = new();

    [ObservableProperty]
    string selectedStage = "Seedling";

    [ObservableProperty]
    string notes = string.Empty;

    [ObservableProperty]
    string heightText = string.Empty;

    [ObservableProperty]
    int healthRating = 3;

    [ObservableProperty]
    bool isSaving = false;

    public List<string> GrowthStages { get; } = new()
    {
        "Seedling",
        "Vegetative",
        "Flowering",
        "Podding",
        "Harvest"
    };

    public List<string> GrowthStagesVietnamese { get; } = new()
    {
        "Nảy mầm",
        "Phát triển lá",
        "Ra hoa",
        "Kết quả",
        "Thu hoạch"
    };

    public List<int> HealthRatings { get; } = new() { 1, 2, 3, 4, 5 };

    public BeanDiaryEntryViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    void SetRating(string ratingStr)
    {
        if (int.TryParse(ratingStr, out int rating))
            HealthRating = rating;
    }

    [RelayCommand]
    async Task SaveEntry()
    {
        if (Plant == null || Plant.Id == 0) return;

        IsSaving = true;
        try
        {
            double? height = null;
            if (double.TryParse(HeightText, out var h))
                height = h;

            var entry = new CreateBeanDiaryEntry
            {
                EntryDate = DateTime.UtcNow,
                GrowthStage = SelectedStage,
                Notes = Notes,
                HeightCm = height,
                HealthRating = HealthRating
            };

            var result = await _apiService.PostAsync<CreateBeanDiaryEntry, BeanDiaryEntry>(
                $"bean/diary/{Plant.Id}", entry);

            if (result != null)
            {
                await App.Current!.MainPage!.DisplayAlert("✅ Thành công", "Đã lưu nhật ký phát triển!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await App.Current!.MainPage!.DisplayAlert("Lỗi", "Không thể lưu nhật ký. Vui lòng thử lại.", "OK");
            }
        }
        catch (Exception ex)
        {
            await App.Current!.MainPage!.DisplayAlert("Lỗi", ex.Message, "OK");
        }
        finally
        {
            IsSaving = false;
        }
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
