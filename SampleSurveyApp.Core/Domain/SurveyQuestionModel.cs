using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyQuestionModel : BaseDatabaseItem
	{
        public string QType { get; set; }

        public string QCode { get; set; }

        public string QText { get; set; }

        public string prevQCode { get; set; }

        public string nextQCode { get; set; }

        public bool IsSelected { get; set; }
    }
}

