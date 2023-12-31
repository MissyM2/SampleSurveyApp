﻿using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyModel : BaseDatabaseItem
	{
        public DateTime SurveyDate { get; set; }
        public string SurveyStatus { get; set; }
        public Guid Suid { get; set; }
        public string SyncStatus { get; set; }
    }
}

