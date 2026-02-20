using Microsoft.Extensions.Logging;
using PlantGuardian.Mobile.Services;
using PlantGuardian.Mobile.ViewModels;
using PlantGuardian.Mobile.Views;

namespace PlantGuardian.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Services
        builder.Services.AddSingleton<ApiService>();

        // Views & ViewModels
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LoginPage>();

        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddSingleton<RegisterPage>();
        
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<DashboardPage>();

        builder.Services.AddSingleton<PlantListViewModel>();
        builder.Services.AddSingleton<PlantListPage>();

        builder.Services.AddTransient<PlantDetailViewModel>();
        builder.Services.AddTransient<PlantDetailPage>();

        builder.Services.AddTransient<BeanViewModel>();
        builder.Services.AddTransient<BeanDetailPage>();

        builder.Services.AddTransient<BeanDiaryEntryViewModel>();
        builder.Services.AddTransient<BeanDiaryEntryPage>();

        builder.Services.AddSingleton<ChatViewModel>();
        builder.Services.AddSingleton<ChatPage>();

		return builder.Build();
	}
}
