using System;
using SampleSurveyApp.Core.Domain;

namespace SampleSurveyApp.Core.Database
{
	public class AddSurveyData
	{
        private readonly IRepository<SurveyValuesModel> _surveyValuesModelRepository;
        public List<SurveyValuesModel> SurveyValuesList { get; set; }

		public AddSurveyData(IRepository<SurveyValuesModel> surveyValuesModelRepository)
		{
            _surveyValuesModelRepository = surveyValuesModelRepository;
            SurveyValuesList = new List<SurveyValuesModel>()
            {
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Have you confirmed the SP is still living at this address?", ValueCode = "Q1", ValueType = "", QuestionType = "List", RuleType = "Single", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Is the SP alive?", ValueCode = "Q2", ValueType = "", QuestionType = "List", RuleType = "Single", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Is the SP residing in a nursing home or nursing home unit?", ValueCode = "Q3", QuestionType = "List", ValueType = "", RuleType = "Single", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Select contact type:", ValueCode = "Q4", ValueType = "", QuestionType = "List", RuleType = "Single", Version = "1", Order = 4, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, and looking around in every direction, how much of the following do you see? ", QuestionType = "List", ValueCode = "Q5a", ValueType = "", RuleType = "Multiple", Version = "1", Order = 5, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, and looking around in every direction, how much of the following do you see? - Graffiti on buildings and walls?", QuestionType = "List", ValueCode = "Q5b", ValueType = "", RuleType = "Single", Version = "1", Order = 6, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, and looking around in every direction, how much of the following do you see? - Vacant or deserted houses or storefronts?", QuestionType = "List", ValueCode = "Q5c", ValueType = "", RuleType = "Single", Version = "1", Order = 7, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, does it have... - Any broken or boarded up windows?", QuestionType = "List", ValueCode = "Q6a", ValueType = "", RuleType = "Single", Version = "1", Order = 8, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, does it have... - A crumbling foundation or open holes?", QuestionType = "List", ValueCode = "Q6b", ValueType = "", RuleType = "Single", Version = "1", Order = 9, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, does it have... - Missing bricks, siding, or other outside materials?", QuestionType = "List", ValueCode = "Q6c", ValueType = "", RuleType = "Single", Version = "1", Order = 10, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, does it have... - Roof problems (e.g. missing material, sagging or a hole in the roof?", QuestionType = "List", ValueCode = "Q6d", ValueType = "", RuleType = "Single", Version = "1", Order = 11, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, does it have... - Uneven walking surfaces or broken steps in the area leading to the home/building?", QuestionType = "List", ValueCode = "Q6e", ValueType = "", RuleType = "Single", Version = "1", Order = 12, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Standing in front of the SP's home/building, does it have... - Continuous sidewalks in both directions?", QuestionType = "List", ValueCode = "Q6f", ValueType = "", RuleType = "Single", Version = "1", Order = 13, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Questions", ValueText = "Any additional comments", QuestionType = "Text", ValueCode = "Q7", ValueType = "", RuleType = "", Version = "1", Order = 13, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q1", RuleType = "Q2", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No - Confirm SP residence before completing Q", ValueCode = "2", ValueType = "Q1", RuleType = "DONE", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q2", RuleType = "Q3", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No", ValueCode = "2", ValueType = "Q2", RuleType = "DONE", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q3", RuleType = "Q4", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No", ValueCode = "2", ValueType = "Q3", RuleType = "Q4", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Online Interview", ValueCode = "1", ValueType = "Q4", RuleType = "Q5a", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "In Person Interview", ValueCode = "2", ValueType = "Q4", RuleType = "Q5a", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Litter on the ground", ValueCode = "17", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Broken glass", ValueCode = "18", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Trash on sidewalks and streets", ValueCode = "19", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Broken window", ValueCode = "20", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 4, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Missing bricks", ValueCode = "21", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 5, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 1", ValueCode = "1", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 2", ValueCode = "2", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 3", ValueCode = "3", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 4", ValueCode = "4", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 4, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 5", ValueCode = "5", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 5, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 6", ValueCode = "6", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 7", ValueCode = "7", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 8", ValueCode = "8", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 9", ValueCode = "9", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 4, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 10", ValueCode = "10", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 5, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 11", ValueCode = "11", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 12", ValueCode = "12", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 13", ValueCode = "13", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 14", ValueCode = "14", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 4, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 15", ValueCode = "15", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 5, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "additional answer 16", ValueCode = "16", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Order = 5, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "None", ValueCode = "1", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "A little", ValueCode = "2", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Some", ValueCode = "3", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "A lot", ValueCode = "4", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Order = 4, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Could not observe", ValueCode = "5", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Order = 5, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "None", ValueCode = "1", ValueType = "Q5c", RuleType = "Done", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "A little", ValueCode = "2", ValueType = "Q5c", RuleType = "Q6a", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Some", ValueCode = "3", ValueType = "Q5c", RuleType = "Q6a", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "A lot", ValueCode = "4", ValueType = "Q5c", RuleType = "Q6a", Version = "1", Order = 4, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Could not observe", ValueCode = "5", ValueType = "Q5c", RuleType = "Q6a", Version = "1", Order = 5, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q6a", RuleType = "Q6b", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No", ValueCode = "2", ValueType = "Q6a", RuleType = "Q6b", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Could not observe", ValueCode = "5", ValueType = "Q6a", RuleType = "Q6b", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q6b", RuleType = "Q6c", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No", ValueCode = "2", ValueType = "Q6b", RuleType = "Q6c", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Could not observe", ValueCode = "5", ValueType = "Q6b", RuleType = "Q6c", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q6c", RuleType = "Q6d", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No", ValueCode = "2", ValueType = "Q6c", RuleType = "Q6d", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Could not observe", ValueCode = "5", ValueType = "Q6c", RuleType = "Q6d", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q6d", RuleType = "Q6e", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No", ValueCode = "2", ValueType = "Q6d", RuleType = "Q6e", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Could not observe", ValueCode = "5", ValueType = "Q6d", RuleType = "Q6e", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q6e", RuleType = "Q6f", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No", ValueCode = "2", ValueType = "Q6e", RuleType = "Q6f", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Could not observe", ValueCode = "5", ValueType = "Q6e", RuleType = "Q6f", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Yes", ValueCode = "1", ValueType = "Q6f", RuleType = "Q7", Version = "1", Order = 1, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "No", ValueCode = "2", ValueType = "Q6f", RuleType = "Q7", Version = "1", Order = 2, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "Could not observe", ValueCode = "5", ValueType = "Q6f", RuleType = "Q7", Version = "1", Order = 3, Timestamp = DateTime.Now },
                new SurveyValuesModel () { SurveyValueType = "Answers", ValueText = "", ValueCode = "1", ValueType = "Q7", RuleType = "DONE", Version = "1", Order = 1, Timestamp = DateTime.Now }
            };
        }

        public async Task AddSurveyValuesAsync()
        {
            try
            {
                _surveyValuesModelRepository.DeleteAllAsync();

                foreach (var surveyvalue in SurveyValuesList)
                {
                    await _surveyValuesModelRepository.SaveAsync(new SurveyValuesModel()
                    {
                        Id = surveyvalue.Id,
                        SurveyValueType = surveyvalue.SurveyValueType,
                        ValueText=surveyvalue.ValueText,
                        ValueCode = surveyvalue.ValueCode,
                        ValueType = surveyvalue.ValueType,
                        RuleType = surveyvalue.RuleType,
                        QuestionType = surveyvalue.QuestionType,
                        Version = surveyvalue.Version,
                        Order = surveyvalue.Order,
                        Timestamp = surveyvalue.Timestamp
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

