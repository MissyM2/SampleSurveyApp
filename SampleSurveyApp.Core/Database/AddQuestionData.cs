using System;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.Services;

namespace SampleSurveyApp.Core.Database
{
	public class AddQuestionData
	{
        private readonly IAsyncRepository<SurveyQuestionModel> _surveyQuestionModelRepository;
        public List<SurveyQuestionModel> SurveyQuestionList { get; set; }
        

        public AddQuestionData(IAsyncRepository<SurveyQuestionModel> surveyQuestionModelRepository)
		{
            _surveyQuestionModelRepository = surveyQuestionModelRepository;

            SurveyQuestionList = new List<SurveyQuestionModel>()
            {
                new SurveyQuestionModel () { QText = "Have you confirmed the SP is still living at this address?", QCode = "Q1", ValueType = "", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Is the SP alive?", QCode  = "Q2", ValueType = "", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Is the SP residing in a nursing home or nursing home unit?", QCode  = "Q3", QType = "List", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Select contact type:", QCode  = "Q4", ValueType = "", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, and looking around in every direction, how much of the following do you see? ", QType = "List", QCode  = "Q5a", ValueType = "", RuleType = "Multiple", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, and looking around in every direction, how much of the following do you see? - Graffiti on buildings and walls?", QType = "List", QCode  = "Q5b", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, and looking around in every direction, how much of the following do you see? - Vacant or deserted houses or storefronts?", QType = "List", QCode  = "Q5c", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, does it have... - Any broken or boarded up windows?", QType = "List", QCode  = "Q6a", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, does it have... - A crumbling foundation or open holes?", QType = "List", QCode  = "Q6b", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, does it have... - Missing bricks, siding, or other outside materials?", QType = "List", QCode  = "Q6c", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, does it have... - Roof problems (e.g. missing material, sagging or a hole in the roof?", QType = "List", QCode  = "Q6d", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, does it have... - Uneven walking surfaces or broken steps in the area leading to the home/building?", QType = "List", QCode  = "Q6e", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Standing in front of the SP's home/building, does it have... - Continuous sidewalks in both directions?", QType = "List", QCode  = "Q6f", ValueType = "", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QText = "Any additional comments", QType = "Text", QCode  = "Q7", ValueType = "", RuleType = "", Version = "1", Timestamp = DateTime.Now }
            };
        }

        
        public async Task AddQuestionsAsync()
        {
            try
            {
                await _surveyQuestionModelRepository.DeleteAllAsync();

                foreach (var item in SurveyQuestionList)
                {
                    await _surveyQuestionModelRepository.InsertAsync(new SurveyQuestionModel()
                    {
                        Id = item.Id,
                        QText=item.QText,
                        QCode = item.QCode,
                        prevQCode="",
                        nextQCode="",
                        ValueType = item.ValueType,
                        RuleType = item.RuleType,
                        QType = item.QType,
                        Version = item.Version,
                        Timestamp = item.Timestamp
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

       

    }
}

