namespace PlantGuardian.Mobile;

/// <summary>
/// Central configuration for backend service URLs.
/// Change these URLs after deploying to Railway / any cloud.
/// </summary>
public static class AppConfig
{
    // ─── .NET API (ASP.NET Core) ─────────────────────────────────────────────
    // Local (Android emulator)  : http://10.0.2.2:5000/api/
    // Local (Windows PC)        : http://localhost:5000/api/
    // Railway cloud             : https://plantguardian-api.up.railway.app/api/
    public const string ApiBaseUrl = "http://10.0.2.2:5000/api/";

    // ─── Python AI Service (FastAPI) ─────────────────────────────────────────
    // Local (Android emulator)  : http://10.0.2.2:8000/
    // Local (Windows PC)        : http://localhost:8000/
    // Railway cloud             : https://plantguardian-ai.up.railway.app/
    public const string AiBaseUrl = "http://10.0.2.2:8000/";
}
