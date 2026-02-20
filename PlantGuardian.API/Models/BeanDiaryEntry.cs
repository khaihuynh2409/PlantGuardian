using System.ComponentModel.DataAnnotations;

namespace PlantGuardian.API.Models
{
    public class BeanDiaryEntry
    {
        public int Id { get; set; }

        [Required]
        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;

        public int UserId { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        /// <summary>Seedling, Vegetative, Flowering, Podding, Harvest</summary>
        public string GrowthStage { get; set; } = "Seedling";

        public string Notes { get; set; } = string.Empty;

        /// <summary>Height of the plant in centimeters</summary>
        public double? HeightCm { get; set; }

        /// <summary>Health rating from 1 (poor) to 5 (excellent)</summary>
        public int HealthRating { get; set; } = 3;
    }
}
