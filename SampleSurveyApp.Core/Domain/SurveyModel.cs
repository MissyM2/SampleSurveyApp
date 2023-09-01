using System;
using SampleSurveyApp.Core.Database;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyModel : BaseDatabaseItem
	{
        public DateTime SurveyDate { get; set; }
        public string SurveyStatus { get; set; }
        public long Suid { get; set; }
        public string SyncStatus { get; set; }
    }
}

