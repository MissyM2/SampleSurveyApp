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
            new AsyncRepository<SurveyValuesModel>(),
            new AsyncRepository<SurveyModel>(),
            new AsyncRepository<SurveyResponseModel>());
    }

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();
    //    await BindingContext.LoadInitialQuestion();
    //}
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        var vm = (SurveyPageVM)BindingContext;
        await vm.LoadInitialQuestion();
    }


}
