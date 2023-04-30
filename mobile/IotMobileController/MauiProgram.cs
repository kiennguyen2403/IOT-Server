using IotMobileController.ViewModels;
using Microsoft.Extensions.Logging;
using IotMobileController.Services;
using System.Net.WebSockets;
using Syncfusion.Maui.Core.Hosting;



namespace IotMobileController;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
			.UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<HttpService>();
		builder.Services.AddSingleton<WebSocketService>();

		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton<ControllerViewModel>();
		builder.Services.AddSingleton<Controller>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
