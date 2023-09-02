using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Services;

namespace SampleSurveyApp.Maui;

public partial class MainPage : ContentPage
{

    public MainPage()
	{
		InitializeComponent();

        BindingContext = new MainPageVM(
          new NavigationService(),
          new MessageService(),
          new UserPreferences(),
          new AsyncRepository<SurveyValuesModel>(),
          new AsyncRepository<SurveyModel>(),
          new AsyncRepository<SurveyResponseModel>());
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        var vm = (MainPageVM)BindingContext;
        await vm.RefreshCommand.ExecuteAsync(null);
    }


}


