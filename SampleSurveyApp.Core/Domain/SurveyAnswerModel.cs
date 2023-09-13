using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyAnswerModel : BaseDatabaseItem
	{
        public string AText { get; set; }

        public string ACode { get; set; }

        public string QCode { get; set; }

        public string RuleType { get; set; }

        public string NotSureWhatThisIs { get; set; }

        public string Version { get; set; }

        public bool IsSelected { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

