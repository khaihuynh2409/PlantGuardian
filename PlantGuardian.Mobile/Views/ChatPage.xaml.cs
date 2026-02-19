using PlantGuardian.Mobile.ViewModels;

namespace PlantGuardian.Mobile.Views;

public partial class ChatPage : ContentPage
{
	public ChatPage(ChatViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
