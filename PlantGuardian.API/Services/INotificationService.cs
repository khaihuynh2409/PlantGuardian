namespace PlantGuardian.API.Services
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(string deviceToken, string title, string body);
        Task<bool> SendBroadcastAsync(string title, string body);
    }
}
