using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SampleSurveyApp.Core.Services;

namespace SampleSurveyApp.Core.ViewModels.Base
{
	public partial class BaseVM : ObservableObject
	{

        [ObservableProperty]
        string pageTitle;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;


        public bool IsNotBusy => !IsBusy;

        public virtual Task InitializeAsync(object parameter)
        {
            return Task.CompletedTask;
        }

    }
}

