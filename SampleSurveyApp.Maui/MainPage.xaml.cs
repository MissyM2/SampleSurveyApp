﻿using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.ViewModels;
using SampleSurveyApp.Maui.Services;

namespace SampleSurveyApp.Maui;

public partial class MainPage : ContentPage
{

    public MainPage()
	{
		InitializeComponent();

        BindingContext = new MainPageVM(
          new NavigationService(),
          new MessageService(),
          new UserPreferences(),
          new Repository<SurveyValuesModel>(),
          new Repository<SurveyModel>(),
          new Repository<SurveyResponseModel>());
    }

	
}


