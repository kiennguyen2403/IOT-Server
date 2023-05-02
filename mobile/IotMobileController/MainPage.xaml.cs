using IotMobileController.ViewModels;

namespace IotMobileController;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

