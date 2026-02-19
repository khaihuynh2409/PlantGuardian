namespace PlantGuardian.Mobile;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Set MainPage to AppShell for Navigation
		MainPage = new AppShell();
	}
}
