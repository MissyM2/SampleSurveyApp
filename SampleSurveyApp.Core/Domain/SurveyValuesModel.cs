﻿using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyValuesModel : BaseDatabaseItem
	{
        public string SurveyValueType { get; set; }

        public string ValueText { get; set; }

        public string ValueCode{ get; set; }

        public string ValueType { get; set; }

        public string RuleType { get; set; }

        public string QuestionType { get; set; }

        public string Version { get; set; }

        public int? Order { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
