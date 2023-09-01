﻿using System;
namespace SampleSurveyApp.Core.Services
{
	public interface IMessageService
	{
        Task DisplayAlert(string title, string message, string cancel);
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
        Task<string> DisplayPrompt(string title, string message, string accept, string cancel);
    }
}

