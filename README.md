# Plant Guardian

Plant Guardian is a comprehensive plant care application featuring a .NET MAUI mobile app, an ASP.NET Core Backend, and a Python-based AI microservice for plant analysis.

## Prerequisites

-   **.NET 9.0 SDK** (or later) (Check with `dotnet --version`)
-   **Python 3.10+** (Check with `python --version`)
-   **SQL Server** (LocalDB or full instance)
-   **.NET MAUI Workload** (`dotnet workload install maui`)

---

## 🚀 How to Run

### 1. Backend API (ASP.NET Core)
The backend handles user data, authentication, and plant management.

1.  Navigate to the API folder:
    ```bash
    cd PlantGuardian.API
    ```
2.  Update `appsettings.json` with your connection strings and API keys (OpenWeatherMap).
3.  Apply Database Migrations:
    ```bash
    dotnet ef database update
    ```
4.  Run the application:
    ```bash
    dotnet run
    ```
    *The API will be available at `http://localhost:5000` (Swagger UI at `http://localhost:5000/swagger`).*

### 2. AI Microservice (Python)
The AI service handles image analysis and chat features.

1.  Navigate to the AI folder:
    ```bash
    cd PlantGuardian.AI
    ```
2.  Create and activate a virtual environment (optional but recommended):
    ```bash
    python -m venv venv
    # Windows:
    .\venv\Scripts\activate
    # Mac/Linux:
    source venv/bin/activate
    ```
3.  Install dependencies:
    ```bash
    pip install -r requirements.txt
    ```
4.  Set Environment Variables:
    Create a `.env` file with `GEMINI_API_KEY=your_key_here`.
5.  Run the service:
    ```bash
    uvicorn main:app --reload --port 8000
    ```
    *The AI Service will be running at `http://localhost:8000` (Docs at `http://localhost:8000/docs`).*

### 3. Mobile App (.NET MAUI)
**Critical Note:** The source code provided relies on the MAUI workload.

1.  **Install Workload**:
    ```bash
    dotnet workload install maui
    ```
2.  **Restore Missing Platform Files**:
    As the project files were generated manually, you must restore the platform-specific folders (`Platforms`, `Resources`) by creating a temporary project:
    ```bash
    dotnet new maui -n TempProject
    # Copy 'Platforms', 'Properties', and 'Resources' folders from TempProject to PlantGuardian.Mobile folder, overwriting if needed.
    ```
3.  **Run the App**:
    ```bash
    cd PlantGuardian.Mobile
    dotnet build
    # Run on Windows
    dotnet run -f net10.0-windows10.0.19041.0
    # To run on Android Emulator, open the solution in Visual Studio 2022 and select the emulator.
    ```

## 🔐 Configuration Keys

-   **Database**: Default connection string points to `(localdb)\mssqllocaldb`. 
-   **API Keys**:
    -   `OpenWeatherMap:ApiKey` -> Add to `PlantGuardian.API/appsettings.json`.
    -   `GEMINI_API_KEY` -> Add to `PlantGuardian.AI/.env`.

## Architecture Overview
-   **PlantGuardian.API**: 
    -   .NET 10 Web API.
    -   Entity Framework Core (SQL Server).
    -   JWT Authentication.
    -   Services: Weather, Notification, Gamification.
-   **PlantGuardian.AI**: 
    -   Python FastAPI.
    -   OpenCV for Image Processing.
    -   Google Gemini for Chat/AI Advice.
-   **PlantGuardian.Mobile**: 
    -   .NET MAUI.
    -   MVVM pattern with CommunityToolkit.
    -   HttpClient for API communication.
