﻿using System;
using SampleSurveyApp.Core.Domain.Base;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyResponseModel : BaseDatabaseItem
	{
        public Guid SurveyId { get; set; }

        public int CurrQCode { get; set; }

        public string QText{ get; set; }

        public int ACode { get; set; }

        public string AText { get; set; }

    }
}

 