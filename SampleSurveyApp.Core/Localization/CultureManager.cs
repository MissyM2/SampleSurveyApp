using Microsoft.Maui.Platform;
using System.Diagnostics;
using System.Globalization;

namespace SampleSurveyApp.Core.Localization
{
    public class CultureManager
    {
        public async Task ChangeLang(string currCulture)
        {
            
            if (currCulture == "en-US")
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo("es-ES");
                CultureInfo.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
            else
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
                CultureInfo.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }


            LocalizationResourceManager.Instance.SetCulture(CultureInfo.DefaultThreadCurrentCulture);
        }

    }
}