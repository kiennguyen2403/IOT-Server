using IotMobileController.ViewModels;

namespace IotMobileController;

public partial class Controller : ContentPage
{
    readonly ControllerViewModel _viewModel;
	public Controller(ControllerViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
		BindingContext = _viewModel;
	}

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _viewModel.SetIsTimerAvailableCommand.Execute(e.Value);
    }

    private void Switch_Toggled_Bedroom(object sender, ToggledEventArgs e)
    {
        _viewModel.SetBedRoomDataCommand.Execute(e.Value);
    }

    private void Switch_Toggled_LivingRoom(object sender, ToggledEventArgs e)
    {
        _viewModel.SetBedRoomDataCommand.Execute(e.Value);
    }

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {


    }
}