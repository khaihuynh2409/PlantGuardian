using System.ComponentModel.DataAnnotations;

namespace PlantGuardian.API.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Email { get; set; } = string.Empty;
        
        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Plant> Plants { get; set; } = new List<Plant>();
    }
}
