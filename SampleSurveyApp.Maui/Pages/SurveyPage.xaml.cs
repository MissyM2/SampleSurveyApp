using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Services;

namespace SampleSurveyApp.Maui.Pages;

public partial class SurveyPage : ContentPage
{
	public SurveyPage()
	{
		InitializeComponent();
        BindingContext = new SurveyPageVM(
            new NavigationService(),
            new MessageService(),
            new UserPreferences(),
            new Repository<SurveyValuesModel>(),
            new Repository<SurveyModel>());
    }

    
}
