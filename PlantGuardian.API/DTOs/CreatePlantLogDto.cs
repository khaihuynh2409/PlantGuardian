using System.ComponentModel.DataAnnotations;

namespace PlantGuardian.API.DTOs
{
    public class CreatePlantLogDto
    {
        [Required]
        public int PlantId { get; set; }

        public string SoilMoistureStatus { get; set; } = string.Empty;

        public string HealthStatus { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public string? ImageUrl { get; set; }
    }
}
