using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyQuestionModel : BaseDatabaseItem
	{
        public string QType { get; set; }

        public int CurrQCode { get; set; }

        public string CurrQCodeDesc { get; set; }
        public string CurrQCodeDescLocal { get; set; }  

        public string QText { get; set; }

        public string QTextLocal { get; set; }

        public int PrevQCode { get; set; }

        public int NextQCode { get; set; }

        public bool IsSelected { get; set; }
    }
}

