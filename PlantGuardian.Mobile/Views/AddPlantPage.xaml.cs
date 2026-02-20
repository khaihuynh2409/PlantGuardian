using PlantGuardian.Mobile.ViewModels;

namespace PlantGuardian.Mobile.Views;

public partial class AddPlantPage : ContentPage
{
    public AddPlantPage(AddPlantViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
