using System.ComponentModel;
using System.Globalization;

namespace SampleSurveyApp.Core.Localization
{
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public static LocalizationResourceManager Instance { get; } = new();
        private LocalizationResourceManager()
        {
            AppResources.Culture = CultureInfo.CurrentCulture;
        }

        

        public object this[string resourceKey] =>
            AppResources.ResourceManager.GetObject(resourceKey, AppResources.Culture) ?? Array.Empty<byte>();

        public void SetCulture(CultureInfo culture)
        {
            //AppResources.Culture = culture;

            AppResources.Culture =
            CultureInfo.DefaultThreadCurrentCulture =
            CultureInfo.DefaultThreadCurrentUICulture =
            CultureInfo.CurrentCulture =
            CultureInfo.CurrentUICulture =
            culture;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));


        }
    }
}
