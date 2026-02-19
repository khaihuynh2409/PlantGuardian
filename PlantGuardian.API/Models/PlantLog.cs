using System.ComponentModel.DataAnnotations;

namespace PlantGuardian.API.Models
{
    public class PlantLog
    {
        public int Id { get; set; }
        
        public DateTime LogDate { get; set; } = DateTime.UtcNow;
        
        public string? ImageUrl { get; set; }
        
        public string SoilMoistureStatus { get; set; } = string.Empty; // Wet, Dry, Moist
        
        public string HealthStatus { get; set; } = string.Empty; // Healthy, Disease detected, etc.
        
        public string? Notes { get; set; }

        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;
    }
}
