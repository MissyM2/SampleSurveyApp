using System;
using SampleSurveyApp.Core.Services;

namespace SampleSurveyApp.Maui.Services
{
	public class MessageService : IMessageService
	{
        public async Task CustomAlert(string title, string message, string accept)
        {
            await Shell.Current.DisplayAlert(title, message, accept);
        }

        public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
           return Shell.Current.DisplayAlert(title, message, accept, cancel);
        }

        public Task<string> DisplayPrompt(string title, string message, string accept, string cancel)
        {
            return Shell.Current.DisplayPromptAsync(title, message, accept, cancel);
        }
    }
}

