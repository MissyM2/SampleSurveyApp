using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyAnswerModel : BaseDatabaseItem
	{
        public int QCode { get; set; }

        public int ACode { get; set; }

        public string AText { get; set; }

        public int RuleType { get; set; }

        public bool IsSelected { get; set; }
    }
}

