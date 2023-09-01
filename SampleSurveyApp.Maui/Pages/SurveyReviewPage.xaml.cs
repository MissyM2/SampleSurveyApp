using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Services;

namespace SampleSurveyApp.Maui.Pages;

public partial class SurveyReviewPage : ContentPage
{
	public SurveyReviewPage()
	{
        InitializeComponent();
        BindingContext = new SurveyReviewPageVM(
           new NavigationService(),
           new MessageService(),
           new UserPreferences(),
           new Repository<SurveyResponseModel>());
    }
}
