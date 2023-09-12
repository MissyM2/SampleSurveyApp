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
                new SurveyAnswerModel () { QType = "Q1", AText = "Yes", ACode = "1", RuleType = "Q2", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q1", AText = "No", ACode = "2", RuleType = "DONE", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q2", AText = "Yes", ACode = "1", RuleType = "Q3", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q2", AText = "No", ACode = "2", RuleType = "DONE", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q3", AText = "Yes", ACode = "1", RuleType = "Q4", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q3", AText = "No", ACode = "2", RuleType = "Q4", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q4", AText = "Option 1", ACode = "1", RuleType = "Q5a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q4", AText = "Option 2", ACode = "2", RuleType = "DONE", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 1", ACode = "1", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 2", ACode = "2", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 3", ACode = "3", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 4", ACode = "4", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 5", ACode = "5", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 6", ACode = "6",RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 7", ACode = "7", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 8", ACode = "8", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 9", ACode = "9", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 10", ACode = "10", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 11", ACode = "11", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 12", ACode = "12", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 13", ACode = "13", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 14", ACode = "14",RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 15", ACode = "15", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 16", ACode = "16", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 17", ACode = "17",RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5a", AText = "Option 18", ACode = "18", RuleType = "Q5b", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q5b", AText = "None", ACode = "1", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5b", AText = "A little", ACode = "2", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5b", AText = "Some", ACode = "3", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5b", AText = "A lot", ACode = "4", RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5b", AText = "Could not observe",  ACode = "5",RuleType = "Q5c", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q5c", AText = "None", ACode = "1", RuleType = "Done", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5c", AText = "A little", ACode = "2",RuleType = "Q6a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5c", AText = "Some", ACode = "3", RuleType = "Q6a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5c", AText = "A lot", ACode = "4", RuleType = "Q6a", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q5c", AText = "Could not observe", ACode = "5", RuleType = "Q6a", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q6a", AText = "Yes", ACode = "1", RuleType = "Q6b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6a", AText = "No", ACode = "2", RuleType = "Q6b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6a", AText = "Could not observe", ACode = "5", RuleType = "Q6b", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6b", AText = "Yes", ACode = "1", RuleType = "Q6c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6b", AText = "No", ACode = "2", RuleType = "Q6c", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q6b", AText = "Could not observe", ACode = "5", RuleType = "Q6c", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { RuleType = "Q6d", AText = "Yes", ACode = "1", QType = "Q6c", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q6c", AText = "No", ACode = "2", RuleType = "Q6d", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6c", AText = "Could not observe", ACode = "5", RuleType = "Q6d", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q6d", AText = "Yes", ACode = "1", RuleType = "Q6e", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6d", AText = "No", ACode = "2", RuleType = "Q6e", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6d", AText = "Could not observe", ACode = "5", RuleType = "Q6e", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q6e", AText = "Yes", ACode = "1", RuleType = "Q6f", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6e", AText = "No", ACode = "2", RuleType = "Q6f", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6e", AText = "Could not observe", ACode = "5", RuleType = "Q6f", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q6f", AText = "Yes", ACode = "1", RuleType = "Q7", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6f", AText = "No", ACode = "2", RuleType = "Q7", Version = "1", Timestamp = DateTime.Now },
                new SurveyAnswerModel () { QType = "Q6f", AText = "Could not observe", ACode = "5", RuleType = "Q7", Version = "1", Timestamp = DateTime.Now },

                new SurveyAnswerModel () { QType = "Q7", AText = "", ACode = "1", RuleType = "DONE", Version = "1", Timestamp = DateTime.Now }
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
                        QType = item.QType,
                        RuleType = item.RuleType,
                        Version = item.Version,
                        IsSelected = false,
                        Timestamp = item.Timestamp
                    }); ;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }


    }
}

