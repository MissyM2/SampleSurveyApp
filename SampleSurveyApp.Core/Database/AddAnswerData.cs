using System;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.Services;

namespace SampleSurveyApp.Core.Database
{
	public class AddAnswerData
	{
        private readonly IAsyncRepository<SurveyAnswerModel> _surveyAnswerModelRepository;
        public List<SurveyAnswerModel> SurveyAnswerList { get; set; }


        public AddAnswerData(IAsyncRepository<SurveyAnswerModel> surveyAnswerModelRepository)
        {
            _surveyAnswerModelRepository = surveyAnswerModelRepository;
            SurveyAnswerList = new List<SurveyAnswerModel>()
            {
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q1", RuleType = "Q2", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No - Confirm SP residence before completing Q", ACode = "2", ValueType = "Q1", RuleType = "DONE", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q2", RuleType = "Q3", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No", ACode = "2", ValueType = "Q2", RuleType = "DONE", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q3", RuleType = "Q4", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No", ACode = "2", ValueType = "Q3", RuleType = "Q4", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Online Interview", ACode = "1", ValueType = "Q4", RuleType = "Q5a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "In Person Interview", ACode = "2", ValueType = "Q4", RuleType = "Q5a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Litter on the ground", ACode = "17", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Broken glass", ACode = "18", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Trash on sidewalks and streets", ACode = "19", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Broken window", ACode = "20", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Missing bricks", ACode = "21", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 1", ACode = "1", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 2", ACode = "2", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 3", ACode = "3", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 4", ACode = "4", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 5", ACode = "5", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 6", ACode = "6", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 7", ACode = "7", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 8", ACode = "8", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 9", ACode = "9", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 10", ACode = "10", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 11", ACode = "11", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 12", ACode = "12", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 13", ACode = "13", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 14", ACode = "14", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 15", ACode = "15", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "additional answer 16", ACode = "16", ValueType = "Q5a", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "None", ACode = "1", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "A little", ACode = "2", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Some", ACode = "3", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "A lot", ACode = "4", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Could not observe", ACode = "5", ValueType = "Q5b", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "None", ACode = "1", ValueType = "Q5c", RuleType = "Done", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "A little", ACode = "2", ValueType = "Q5c", RuleType = "Q6a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Some", ACode = "3", ValueType = "Q5c", RuleType = "Q6a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "A lot", ACode = "4", ValueType = "Q5c", RuleType = "Q6a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Could not observe", ACode = "5", ValueType = "Q5c", RuleType = "Q6a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q6a", RuleType = "Q6b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No", ACode = "2", ValueType = "Q6a", RuleType = "Q6b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Could not observe", ACode = "5", ValueType = "Q6a", RuleType = "Q6b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q6b", RuleType = "Q6c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No", ACode = "2", ValueType = "Q6b", RuleType = "Q6c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Could not observe", ACode = "5", ValueType = "Q6b", RuleType = "Q6c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q6c", RuleType = "Q6d", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No", ACode = "2", ValueType = "Q6c", RuleType = "Q6d", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Could not observe", ACode = "5", ValueType = "Q6c", RuleType = "Q6d", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q6d", RuleType = "Q6e", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No", ACode = "2", ValueType = "Q6d", RuleType = "Q6e", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Could not observe", ACode = "5", ValueType = "Q6d", RuleType = "Q6e", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q6e", RuleType = "Q6f", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No", ACode = "2", ValueType = "Q6e", RuleType = "Q6f", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Could not observe", ACode = "5", ValueType = "Q6e", RuleType = "Q6f", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Yes", ACode = "1", ValueType = "Q6f", RuleType = "Q7", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "No", ACode = "2", ValueType = "Q6f", RuleType = "Q7", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "Could not observe", ACode = "5", ValueType = "Q6f", RuleType = "Q7", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { AText = "", ACode = "1", ValueType = "Q7", RuleType = "DONE", Version = "1", Timestamp = DateTime.Now }
            };
        }

        public async Task AddAnswersAsync()
        {
            try
            {
                await _surveyAnswerModelRepository.DeleteAllAsync();

                foreach (var item in SurveyAnswerList)
                {
                    await _surveyAnswerModelRepository.InsertAsync(new SurveyAnswerModel()
                    {
                        Id = item.Id,
                        AText = item.AText,
                        ACode = item.ACode,
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

