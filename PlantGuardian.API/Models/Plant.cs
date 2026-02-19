using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantGuardian.API.Models
{
    public class Plant
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Species { get; set; } = string.Empty;
        
        public string PlantType { get; set; } = string.Empty; // e.g., Succulent, Cactus
        
        public DateTime DatePlanted { get; set; }
        
        public DateTime? LastWatered { get; set; }
        
        public string SoilType { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<PlantLog> Logs { get; set; } = new List<PlantLog>();
    }
}
