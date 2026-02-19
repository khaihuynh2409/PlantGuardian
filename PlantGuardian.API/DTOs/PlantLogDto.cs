namespace PlantGuardian.API.DTOs
{
    public class PlantLogDto
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string? ImageUrl { get; set; }
        public string SoilMoistureStatus { get; set; } = string.Empty;
        public string HealthStatus { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int PlantId { get; set; }
    }
}
