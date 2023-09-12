using Microsoft.Maui.Controls;
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
            new AsyncRepository<SurveyQuestionModel>(),
            new AsyncRepository<SurveyAnswerModel>(),
            new AsyncRepository<SurveyModel>(),
            new AsyncRepository<SurveyResponseModel>());
        //LoadAfterConstruction();
    }

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();
    //    await BindingContext.LoadInitialQuestion();
    //}
    //protected async override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    var vm = (SurveyPageVM)BindingContext;
    //    await vm.LoadInitialQuestion();
    //}

    //private async void LoadAfterConstruction()
    //{
    //    var vm = (SurveyPageVM)BindingContext;
    //    vm.IsBusy = true;
    //    await vm.LoadInitialQuestion();

    //    //only set the binding of the CollectionView after loading completed
    //    //MyCV.SetBinding(ItemsView.ItemsSourceProperty, nameof(MyCV));

    //    vm.IsBusy = false;
    //}


}
