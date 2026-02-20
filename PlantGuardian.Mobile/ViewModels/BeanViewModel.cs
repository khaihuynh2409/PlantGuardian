using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;
using System.Collections.ObjectModel;

namespace PlantGuardian.Mobile.ViewModels;

[QueryProperty(nameof(Plant), "Plant")]
public partial class BeanViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    Plant plant = new();

    [ObservableProperty]
    BeanCareProfile? careProfile;

    [ObservableProperty]
    ObservableCollection<BeanDiaryEntry> diaryEntries = new();

    [ObservableProperty]
    BeanWateringSchedule? wateringSchedule;

    [ObservableProperty]
    bool isLoading = false;

    [ObservableProperty]
    string beanEmoji = "🌱";

    public BeanViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    partial void OnPlantChanged(Plant value)
    {
        BeanEmoji = value.PlantType switch
        {
            "BlackBean" => "🖤🫘",
            "Soybean"   => "🟡🫘",
            "FavaBean"  => "🟢🫘",
            _            => "🌱"
        };
    }

    [RelayCommand]
    async Task LoadBeanData()
    {
        if (Plant == null || Plant.Id == 0) return;
        IsLoading = true;

        try
        {
            // Load care profile and watering schedule in parallel
            var profileTask = _apiService.GetAsync<BeanCareProfile>($"bean/care-profile/{Plant.PlantType}");
            var schedTask   = _apiService.GetAsync<BeanWateringSchedule>($"bean/watering-schedule/{Plant.Id}");
            var diaryTask   = _apiService.GetAsync<List<BeanDiaryEntry>>($"bean/diary/{Plant.Id}");

            await Task.WhenAll(profileTask, schedTask, diaryTask);

            CareProfile      = await profileTask;
            WateringSchedule = await schedTask;

            var entries = await diaryTask;
            DiaryEntries.Clear();
            if (entries != null)
                foreach (var e in entries)
                    DiaryEntries.Add(e);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task GoToAddDiary()
    {
        await Shell.Current.GoToAsync("BeanDiaryEntryPage", new Dictionary<string, object>
        {
            { "Plant", Plant }
        });
    }

    [RelayCommand]
    async Task AskAIAboutBean()
    {
        string question = await App.Current!.MainPage!.DisplayPromptAsync(
            $"Hỏi AI về {CareProfile?.DisplayName ?? "Đậu"}",
            "Nhập câu hỏi của bạn:",
            placeholder: "Ví dụ: Sao lá cây bị vàng?");

        if (string.IsNullOrEmpty(question)) return;

        IsLoading = true;
        try
        {
            // Use the /bean-advice endpoint in the AI service (port 8000)
            var request = new { plant_type = Plant.PlantType, question, growth_stage = "" };
            var result = await _apiService.PostAsync<object, BeanAdviceResponse>(
                "http://10.0.2.2:8000/bean-advice", request);

            var answer = result?.Advice ?? "Không nhận được phản hồi.";
            await App.Current!.MainPage!.DisplayAlert("Lời khuyên từ AI 🤖", answer, "OK");
        }
        catch (Exception ex)
        {
            await App.Current!.MainPage!.DisplayAlert("Lỗi", $"Không thể kết nối AI: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}

// Helper model for deserializing bean advice response
public class BeanAdviceResponse
{
    public string PlantType { get; set; } = string.Empty;
    public string PlantName { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public string Advice { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
