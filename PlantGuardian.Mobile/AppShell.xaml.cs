using PlantGuardian.Mobile.Views;

namespace PlantGuardian.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        
        // Register Routes for Navigation
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(PlantDetailPage), typeof(PlantDetailPage));
        Routing.RegisterRoute(nameof(BeanDetailPage), typeof(BeanDetailPage));
        Routing.RegisterRoute(nameof(BeanDiaryEntryPage), typeof(BeanDiaryEntryPage));
        Routing.RegisterRoute(nameof(AddPlantPage), typeof(AddPlantPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
	}
}
