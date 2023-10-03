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
        try
        {
            //Shell.Current.DisplayAlert("Orientation", $"Current Orientation: {DeviceDisplay.Current.MainDisplayInfo.Orientation}", "OK");
            var vm = (SurveyPageVM)BindingContext;
            if (DeviceDisplay.Current.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
            {


                vm.ScreenHeight = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Height;
                vm.ScreenWidth = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;
                vm.IsLandscape = true;
                vm.IsPortrait = false;
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
        catch (Exception ex)
        {
            Debug.WriteLine($"SurveyPage.xaml.cs:Current_MainDisplayInfoChanged: '{ex}'");
        }
        
    }

    //protected async override void OnAppearing()
    //{
    //    base.OnAppearing();

    //    try
    //    {
    //        await Shell.Current.DisplayAlert("Orientation", DeviceDisplay.Current.MainDisplayInfo.Orientation.ToString(), "OK");
    //        var vm = (SurveyPageVM)BindingContext;
    //        vm.ScreenNameLbl = AppResources.ScreenNameLblStart;
    //        Debug.WriteLine($"Normal state active: {vm.IsSelected}");
    //        await vm.Init();

    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine($"SurveyPage.xaml.cs:OnAppearing: '{ex}'");
    //    }
        
    //}

    void OnNormalStateIsActiveChanged(object sender, EventArgs e)
    {
        try
        {
            StateTriggerBase stateTrigger = sender as StateTriggerBase;
            Debug.WriteLine($"Normal state active: {stateTrigger.IsActive}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SurveyPage.xaml.cs:OnNormalStateIsActiveChanged(: '{ex}'");
        }
        
    }

    void OnSelectedStateIsActiveChanged(object sender, EventArgs e)
    {
        try
        {
            StateTriggerBase stateTrigger = sender as StateTriggerBase;
            Debug.WriteLine($"Selected state active: {stateTrigger.IsActive}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SurveyPage.xaml.cs:OnSelectedStateIsActiveChanged(: '{ex}'");
        }

    }
}
