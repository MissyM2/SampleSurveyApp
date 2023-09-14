using Microsoft.Extensions.Logging;
using SampleSurveyApp.Core.Services;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Pages;
using SampleSurveyApp.Maui.Services;
using CommunityToolkit.Maui;

namespace SampleSurveyApp.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
			.UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IMessageService, MessageService>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageVM>();

        builder.Services.AddSingleton<SurveyPage>();
        builder.Services.AddSingleton<SurveyPageVM>();

        builder.Services.AddSingleton<OverviewPage>();

        builder.Services.AddSingleton<ShellPageVM>();


        return builder.Build();
	}
}

