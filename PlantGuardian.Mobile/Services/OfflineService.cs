using System.Text.Json;

namespace PlantGuardian.Mobile.Services;

public class OfflineService
{
    public async Task SaveDataAsync<T>(string key, T data)
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{key}.json");
        string json = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<T?> LoadDataAsync<T>(string key)
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{key}.json");
        if (!File.Exists(filePath)) return default;

        string json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
}
