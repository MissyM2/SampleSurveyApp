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
                new SurveyQuestionModel () { QCode = "Q1", QText = "Question 1", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q2", QText = "Question 2", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q3", QText = "Question 3", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q4", QText = "Question 4", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q5a", QText = "Question 5", QType = "List", RuleType = "Multiple", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q5b", QText = "Question 6", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q5c", QText = "Question 7", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q6a", QText = "Question 8", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q6b", QText = "Question 9", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q6c", QText = "Question 10", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q6d", QText = "Question 11", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q6e", QText = "Question 12", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q6f", QText = "Question 13", QType = "List", RuleType = "Single", Version = "1", Timestamp = DateTime.Now },
                new SurveyQuestionModel () { QCode  = "Q7", QText = "Question 14", QType = "Text", ValueType = "", RuleType = "", Version = "1", Timestamp = DateTime.Now }
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

