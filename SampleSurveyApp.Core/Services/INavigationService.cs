using System;
namespace SampleSurveyApp.Core.Services
{
	public interface INavigationService
	{
        Task GoBackAsync();
        Task GoToMainPageAsync();
        Task GoToSurveyPageAsync();
    }
}

