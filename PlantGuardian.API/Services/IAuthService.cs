using PlantGuardian.API.Models;

namespace PlantGuardian.API.Services
{
    public interface IAuthService
    {
        Task<User?> Register(User user, string password);
        Task<string?> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
