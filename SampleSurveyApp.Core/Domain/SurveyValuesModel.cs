using System;
using SampleSurveyApp.Core.Database;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyValuesModel : BaseDatabaseItem
	{
        public string NameText { get; set; }

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

