using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace PlantGuardian.Mobile.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private string _authToken = string.Empty;

    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(AppConfig.ApiBaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public void SetToken(string token)
    {
        _authToken = token;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public bool IsLoggedIn => !string.IsNullOrEmpty(_authToken);

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
        catch { return default; }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            if (!response.IsSuccessStatusCode) return default;
            var responseContent = await response.Content.ReadAsStringAsync();
            if (typeof(TResponse) == typeof(string))
                return (TResponse)(object)responseContent;
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }
        catch { return default; }
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            if (!response.IsSuccessStatusCode) return default;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }
        catch { return default; }
    }
}
