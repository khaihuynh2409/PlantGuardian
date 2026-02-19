using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace PlantGuardian.Mobile.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private string _authToken = string.Empty;
    
    // Adjust localhost for Android Emulator (10.0.2.2) vs Windows (localhost)
    private const string BaseUrl = "http://10.0.2.2:5000/api/"; 
    // For Windows machine testing use: "http://localhost:5000/api/";

    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public void SetToken(string token)
    {
        _authToken = token;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode) return default;
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);
        if (!response.IsSuccessStatusCode) return default;

        var responseContent = await response.Content.ReadAsStringAsync();
        
        // Handle case where response is a simple string (token)
        if (typeof(TResponse) == typeof(string))
            return (TResponse)(object)responseContent;

        return JsonConvert.DeserializeObject<TResponse>(responseContent);
    }
}
