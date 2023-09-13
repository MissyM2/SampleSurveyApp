﻿using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyQuestionModel : BaseDatabaseItem
	{
        public string QType { get; set; }

        public int QCode { get; set; }

        public string QCodeDesc { get; set; }

        public string QText { get; set; }

        public int PrevQCode { get; set; }

        public int NextQCode { get; set; }

        public bool IsSelected { get; set; }
    }
}

