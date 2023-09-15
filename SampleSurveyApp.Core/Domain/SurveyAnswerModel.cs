using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SampleSurveyApp.Core.Domain.Base;
using SQLite;

namespace SampleSurveyApp.Core.Domain
{
	public partial class SurveyAnswerModel : BaseDatabaseItem
	{
        //public int QCode { get; set; }

        //public int ACode { get; set; }

        //public string AText { get; set; }

        //public int RuleType { get; set; }

        //public bool IsSelected { get; set; }


        [ObservableProperty]
        int qCode;

        [ObservableProperty]
        int aCode;

        [ObservableProperty]
        string aText;

        [ObservableProperty]
        int ruleType;

        [ObservableProperty]
        bool isSelected;
    }
}

