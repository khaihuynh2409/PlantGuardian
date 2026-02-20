namespace PlantGuardian.Mobile.Models;

public class LoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterModel
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class Plant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string PlantType { get; set; } = string.Empty; // BlackBean, Soybean, FavaBean, etc.
    public string ImageUrl { get; set; } = string.Empty;
    public string SoilType { get; set; } = string.Empty;
    public DateTime? LastWatered { get; set; }
    public DateTime DatePlanted { get; set; }

    public bool IsBean => PlantType is "BlackBean" or "Soybean" or "FavaBean";
}

public class BeanCareProfile
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

public class BeanDiaryEntry
{
    public int Id { get; set; }
    public int PlantId { get; set; }
    public DateTime EntryDate { get; set; }
    public string GrowthStage { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public double? HeightCm { get; set; }
    public int HealthRating { get; set; }
}

public class CreateBeanDiaryEntry
{
    public DateTime EntryDate { get; set; } = DateTime.UtcNow;
    public string GrowthStage { get; set; } = "Seedling";
    public string Notes { get; set; } = string.Empty;
    public double? HeightCm { get; set; }
    public int HealthRating { get; set; } = 3;
}

public class BeanWateringSchedule
{
    public int PlantId { get; set; }
    public string PlantName { get; set; } = string.Empty;
    public string PlantType { get; set; } = string.Empty;
    public DateTime? LastWatered { get; set; }
    public int DaysUntilNextWatering { get; set; }
    public int? WateringFrequencyDays { get; set; }
    public bool NeedsWateringNow { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class WeatherResponse
{
    public MainInfo Main { get; set; } = new();
}

public class MainInfo
{
    public double Temp { get; set; }
    public double Humidity { get; set; }
}

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
    public string Context { get; set; } = string.Empty;
}

public class ChatResponse
{
    public string Response { get; set; } = string.Empty;
}
