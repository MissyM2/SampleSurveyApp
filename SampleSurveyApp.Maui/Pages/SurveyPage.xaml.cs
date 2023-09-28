using CommunityToolkit.Mvvm.ComponentModel;
using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.Localization;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Services;
using System.Diagnostics;
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

        DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;

    }

    private void Current_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        //Shell.Current.DisplayAlert("Orientation", $"Current Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}", "OK");
        var vm = (SurveyPageVM)BindingContext;
        if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
        {

            vm.IsLandscape = true;
            vm.IsPortrait = false;
            vm.ScreenHeight = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Height;
            vm.ScreenWidth = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;
            vm.ScrollViewScreenHeight = vm.ScreenHeight - 200;

        }
        else
        {
            vm.IsLandscape = false;
            vm.IsPortrait = true;
            vm.ScreenHeight = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Height;
            vm.ScreenWidth = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;
            vm.ScrollViewScreenHeight = vm.ScreenHeight - 200;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await Shell.Current.DisplayAlert("Orientation", DeviceDisplay.Current.MainDisplayInfo.Orientation.ToString(), "OK");



        var vm = (SurveyPageVM)BindingContext;
        vm.ScreenNameLbl= AppResources.ScreenNameLblStart;
        Debug.WriteLine($"Normal state active: {vm.IsSelected}");
        await vm.Init();
    }

    void OnNormalStateIsActiveChanged(object sender, EventArgs e)
    {
        StateTriggerBase stateTrigger = sender as StateTriggerBase;
        Debug.WriteLine($"Normal state active: {stateTrigger.IsActive}");
    }

    void OnSelectedStateIsActiveChanged(object sender, EventArgs e)
    {
        StateTriggerBase stateTrigger = sender as StateTriggerBase;
        Debug.WriteLine($"Selected state active: {stateTrigger.IsActive}");
    }
}
