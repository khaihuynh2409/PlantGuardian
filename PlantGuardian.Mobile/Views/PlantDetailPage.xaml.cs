using PlantGuardian.Mobile.ViewModels;

namespace PlantGuardian.Mobile.Views;

public partial class PlantDetailPage : ContentPage
{
	public PlantDetailPage(PlantDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
