using SampleSurveyApp.Maui.Pages;

namespace SampleSurveyApp.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(SurveyPage), typeof(SurveyPage));
    }
}

