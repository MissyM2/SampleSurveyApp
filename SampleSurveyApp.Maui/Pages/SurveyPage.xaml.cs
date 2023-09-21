using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.Localization;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Services;
using System.Globalization;

namespace SampleSurveyApp.Maui.Pages;

public partial class SurveyPage : ContentPage
{
    public LocalizationResourceManager LocalizationResourceManager => LocalizationResourceManager.Instance;

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

    private void languagePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        (BindingContext as SurveyPageVM).OnSelectedIndexChanged(sender, e);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var switchToCulture = AppResources.Culture.TwoLetterISOLanguageName
            .Equals("es", StringComparison.InvariantCultureIgnoreCase) ?
            new CultureInfo("en-US") : new CultureInfo("es-ES");

        LocalizationResourceManager.Instance.SetCulture(switchToCulture);

    }
}
