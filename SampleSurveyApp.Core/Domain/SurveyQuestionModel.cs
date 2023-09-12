using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyQuestionModel : BaseDatabaseItem
	{
        public string QText { get; set; }

        public string QCode { get; set; }

        public string prevQCode { get; set; }

        public string nextQCode { get; set; }

        public string ValueType { get; set; }

        public string RuleType { get; set; }

        public string QType { get; set; }

        public bool IsSelected { get; set; }
    }
}

