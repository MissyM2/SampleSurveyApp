﻿using System;
using System.Threading.Tasks;

namespace SampleSurveyApp.Core.Services
{
	public interface INavigationService
	{
        Task GoBackAsync();
        Task GoToSurveyPageAsync();
    }
}

