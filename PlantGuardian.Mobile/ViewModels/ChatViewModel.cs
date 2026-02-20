using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.Models;
using System.Collections.ObjectModel;

namespace PlantGuardian.Mobile.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    string userMessage;

    [ObservableProperty]
    ObservableCollection<ChatMessage> messages;

    public ChatViewModel(ApiService apiService)
    {
        _apiService = apiService;
        Messages = new ObservableCollection<ChatMessage>
        {
            new ChatMessage { Text = "Hello! I am your Plant Guardian assistant. How can I help you today?", IsBot = true }
        };
    }

    [RelayCommand]
    async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(UserMessage)) return;

        var messageText = UserMessage;
        Messages.Add(new ChatMessage { Text = messageText, IsBot = false });
        UserMessage = string.Empty;

        // Call API
        var request = new ChatRequest { Message = messageText, Context = "User is asking about plants." };
        // NOTE: The endpoint in Python Main.py is "/chat", not "/api/chat" if running standalone, 
        // but here we are using the ApiService which prefixes /api/. 
        // We might need to adjust the Python Service to run under /api prefix or change ApiService.
        // Assuming ApiService BaseUrl is correctly pointing to where Python app is reachable, or Python app is behind a proxy.
        // For simplicity, let's assume direct connection to Python service for CHAT if on different port, 
        // OR we implemented a Gateway. 
        // **Correction**: Python runs on 8000, .NET on 5000. 
        // I will hack the URL here for the Python service specific call.
        
        try 
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(AppConfig.AiBaseUrl); 
             
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync("chat", content);
            if (response.IsSuccessStatusCode)
            {
               var jsonRes = await response.Content.ReadAsStringAsync();
               var chatRes = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonRes);
               string reply = chatRes.response;
               Messages.Add(new ChatMessage { Text = reply, IsBot = true });
            }
            else
            {
                Messages.Add(new ChatMessage { Text = "Sorry, I couldn't reach the brain.", IsBot = true });
            }
        }
        catch (Exception ex)
        {
             Messages.Add(new ChatMessage { Text = $"Error: {ex.Message}", IsBot = true });
        }
    }
}

public class ChatMessage
{
    public string Text { get; set; } = string.Empty;
    public bool IsBot { get; set; }
    public Color Color => IsBot ? Colors.LightGreen : Colors.LightBlue;
    public LayoutOptions Alignment => IsBot ? LayoutOptions.Start : LayoutOptions.End;
}
