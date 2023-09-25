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
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-ES");
                CultureInfo.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
            }
            else
            {
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
                CultureInfo.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
            }


            LocalizationResourceManager.Instance.SetCulture(CultureInfo.DefaultThreadCurrentCulture);
        }

    }
}