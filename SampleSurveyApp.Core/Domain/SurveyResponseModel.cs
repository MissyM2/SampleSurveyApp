using System;
using SampleSurveyApp.Core.Database;

namespace SampleSurveyApp.Core.Domain
{
	public class SurveyResponseModel : BaseDatabaseItem
	{
        public int SurveyId { get; set; }

        public int SequenceNo { get; set; }

        public string QuestionCode { get; set; }

        public string QuestionText{ get; set; }

        public string AnswerCode { get; set; }

        public string AnswerText { get; set; }

    }
}

