using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.Localization;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Pages;
using SampleSurveyApp.Maui.Services;

namespace SampleSurveyApp.Maui;

public partial class AppShell : Shell
{
    
    public AppShell()
	{
		InitializeComponent();
        BindingContext = new ShellPageVM(
            new NavigationService(),
            new MessageService(),
            new AsyncRepository<SurveyModel>(),
            new AsyncRepository<SurveyResponseModel>());

        Routing.RegisterRoute(nameof(SurveyPage), typeof(SurveyPage));
        Routing.RegisterRoute(nameof(OverviewPage), typeof(OverviewPage));
    }
}

