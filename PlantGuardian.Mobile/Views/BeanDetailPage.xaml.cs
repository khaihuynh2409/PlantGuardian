using PlantGuardian.Mobile.ViewModels;

namespace PlantGuardian.Mobile.Views;

public partial class BeanDetailPage : ContentPage
{
    public BeanDetailPage(BeanViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BeanViewModel vm)
            await vm.LoadBeanDataCommand.ExecuteAsync(null);
    }
}
