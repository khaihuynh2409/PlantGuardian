using PlantGuardian.Mobile.ViewModels;

namespace PlantGuardian.Mobile.Views;

public partial class BeanDiaryEntryPage : ContentPage
{
    public BeanDiaryEntryPage(BeanDiaryEntryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
