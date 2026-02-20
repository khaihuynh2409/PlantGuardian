namespace PlantGuardian.API.DTOs
{
    public class BeanCareProfileDto
    {
        public string PlantType { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WateringFrequencyDays { get; set; }
        public double SunlightHoursPerDay { get; set; }
        public string SoilPhRange { get; set; } = string.Empty;
        public string IdealTemperatureCelsius { get; set; } = string.Empty;
        public int GrowthDurationDays { get; set; }
        public string FertilizerSchedule { get; set; } = string.Empty;
        public List<string> CommonDiseases { get; set; } = new();
        public List<string> GrowthStages { get; set; } = new();
        public string HarvestTips { get; set; } = string.Empty;
        public string WateringTips { get; set; } = string.Empty;
    }
}
