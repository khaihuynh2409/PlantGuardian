namespace PlantGuardian.API.DTOs
{
    public class PlantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string PlantType { get; set; } = string.Empty;
        public DateTime DatePlanted { get; set; }
        public DateTime? LastWatered { get; set; }
        public string SoilType { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int UserId { get; set; }
    }
}
