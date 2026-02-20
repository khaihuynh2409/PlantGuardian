namespace PlantGuardian.API.DTOs
{
    public class BeanDiaryEntryDto
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public DateTime EntryDate { get; set; }
        public string GrowthStage { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public double? HeightCm { get; set; }
        public int HealthRating { get; set; }
    }

    public class CreateBeanDiaryEntryDto
    {
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;
        public string GrowthStage { get; set; } = "Seedling";
        public string Notes { get; set; } = string.Empty;
        public double? HeightCm { get; set; }
        public int HealthRating { get; set; } = 3;
    }
}
