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
    public string ImageUrl { get; set; } = string.Empty;
    public string SoilType { get; set; } = string.Empty;
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
