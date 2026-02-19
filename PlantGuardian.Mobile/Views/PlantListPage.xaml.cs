using PlantGuardian.Mobile.ViewModels;

namespace PlantGuardian.Mobile.Views;

public partial class PlantListPage : ContentPage
{
    private readonly PlantListViewModel _vm;
    
	public PlantListPage(PlantListViewModel vm)
	{
		InitializeComponent();
		BindingContext = _vm = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadPlantsCommand.ExecuteAsync(null);
    }
}
