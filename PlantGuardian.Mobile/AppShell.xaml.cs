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
	}
}
