using System;
using SampleSurveyApp.Core.Services;
using SampleSurveyApp.Maui.Pages;

namespace SampleSurveyApp.Maui.Services
{
	public class NavigationService : INavigationService
	{
		public NavigationService()
		{
		}

        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task GoToMainPageAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        public async Task GoToSurveyPageAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(SurveyPage)}");
        }

        public async Task GoToSurveyReviewPageAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(SurveyReviewPage)}");
        }


        // look at this
        //private Task GoToAsync<TViewModel>(string routePrefix, string parameters) where TViewModel : BaseVM
        //{
        //    var route = routePrefix + typeof(TViewModel).Name;
        //    if (!string.IsNullOrWhiteSpace(parameters))
        //    {
        //        route += $"?{parameters}";
        //    }
        //    return Shell.Current.GoToAsync(route);
        //}
    }
}

