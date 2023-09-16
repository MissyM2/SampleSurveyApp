using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SampleSurveyApp.Core.Domain.Base;
using SQLite;

namespace SampleSurveyApp.Core.Domain
{
	public partial class SurveyAnswerModel : BaseDatabaseItem
	{

        [ObservableProperty]
        int currQCode;

        [ObservableProperty]
        int aCode;

        [ObservableProperty]
        string aText;

        [ObservableProperty]
        int navRule;

        [ObservableProperty]
        bool isSelected;
    }
}

