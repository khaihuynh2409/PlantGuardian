using PlantGuardian.API.Data;
using PlantGuardian.API.Models;
using Microsoft.EntityFrameworkCore;

namespace PlantGuardian.API.Services
{
    public interface IGamificationService
    {
        Task<List<string>> GetBadgesAsync(int userId);
        Task UpdateStreakAsync(int userId);
    }

    public class GamificationService : IGamificationService
    {
        private readonly PlantGuardianContext _context;

        public GamificationService(PlantGuardianContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetBadgesAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var badges = new List<string>();

            if (user == null) return badges;

            // Logic to determine badges
            // Example: "New Gardener" for 1 plant
            var plantCount = await _context.Plants.Where(p => p.UserId == userId).CountAsync();
            if (plantCount >= 1) badges.Add("New Gardener");
            if (plantCount >= 5) badges.Add("Green Thumb");
            if (plantCount >= 10) badges.Add("Botanist");

            // Example: "Streak Master" (requires tracking login dates, which User model doesn't fully have yet, assuming logic exists)
            // if (user.Streak > 7) badges.Add("Streak Master");

            return badges;
        }

        public async Task UpdateStreakAsync(int userId)
        {
            // Placeholder for streak update logic
            // Would need to check last login date vs today
            await Task.CompletedTask;
        }
    }
}
