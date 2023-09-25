using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.Localization;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Services;
using System.Globalization;

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
            new AsyncRepository<SurveyQuestionModel>(),
            new AsyncRepository<SurveyAnswerModel>(),
            new AsyncRepository<SurveyModel>(),
            new AsyncRepository<SurveyResponseModel>());
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        var vm = (SurveyPageVM)BindingContext;
        vm.ScreenNameLbl= AppResources.ScreenNameLblStart;
        await vm.Init();
    }
}
