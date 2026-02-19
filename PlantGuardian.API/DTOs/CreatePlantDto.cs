using System.ComponentModel.DataAnnotations;

namespace PlantGuardian.API.DTOs
{
    public class CreatePlantDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;

        public string PlantType { get; set; } = "Other"; // Succulent, Cactus, Carnivorous, Other

        public DateTime DatePlanted { get; set; } = DateTime.UtcNow;

        public string SoilType { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; }
    }
}
