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
                new SurveyAnswerModel () { QCode = 1, AText = "Yes", ACode = 1, RuleType = 21},
                new SurveyAnswerModel () { QCode = 1, AText = "No", ACode = 2, RuleType = -1},

                new SurveyAnswerModel () { QCode = 2, AText = "Yes", ACode = 1, RuleType = 3},
                new SurveyAnswerModel () { QCode = 2, AText = "No", ACode = 2, RuleType = -1},

                new SurveyAnswerModel () { QCode = 3, AText = "Yes", ACode = 1, RuleType = 4},
                new SurveyAnswerModel () { QCode = 3, AText = "No", ACode = 2, RuleType = 4},

                new SurveyAnswerModel () { QCode = 4, AText = "Option 1", ACode = 1, RuleType = 5},
                new SurveyAnswerModel () { QCode = 4, AText = "Option 2", ACode = 2, RuleType = -1},

                new SurveyAnswerModel () { QCode = 5, AText = "Option 1", ACode = 1, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 2", ACode = 2, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 3", ACode = 3, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 4", ACode = 4, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 5", ACode = 5, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 6", ACode = 6,RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 7", ACode = 7, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 8", ACode = 8, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 9", ACode = 9, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 10", ACode = 10, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 11", ACode = 11, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 12", ACode = 12, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 13", ACode = 13, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 14", ACode = 14,RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 15", ACode = 15, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 16", ACode = 16, RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 17", ACode = 17,RuleType = 6},
                new SurveyAnswerModel () { QCode = 5, AText = "Option 18", ACode = 18, RuleType = 6},

                new SurveyAnswerModel () { QCode = 6, AText = "None", ACode = 1, RuleType = 14},
                new SurveyAnswerModel () { QCode = 6, AText = "A little", ACode = 2, RuleType = 14},
                new SurveyAnswerModel () { QCode = 6, AText = "Some", ACode = 3, RuleType = 14},
                new SurveyAnswerModel () { QCode = 6, AText = "A lot", ACode = 4, RuleType = 14},
                new SurveyAnswerModel () { QCode = 6, AText = "Could not observe",  ACode = 5,RuleType = 14},

                new SurveyAnswerModel () { QCode = 7, AText = "Yes", ACode = 1, RuleType = 8},
                new SurveyAnswerModel () { QCode = 7, AText = "No", ACode = 2, RuleType = 8},
                new SurveyAnswerModel () { QCode = 7, AText = "Could not observe", ACode = 5, RuleType = 8},
                new SurveyAnswerModel () { QCode = 8, AText = "Yes", ACode = 1, RuleType = 9},
                new SurveyAnswerModel () { QCode = 8, AText = "No", ACode = 2, RuleType = 9},

                new SurveyAnswerModel () { QCode = 8, AText = "Could not observe", ACode = 5, RuleType = 9},
                new SurveyAnswerModel () { RuleType = 10, AText = "Yes", ACode = 1, QCode = 9},

                new SurveyAnswerModel () { QCode = 9, AText = "No", ACode = 2, RuleType = 10},
                new SurveyAnswerModel () { QCode = 9, AText = "Could not observe", ACode = 5, RuleType = 10},

                new SurveyAnswerModel () { QCode = 10, AText = "Yes", ACode = 1, RuleType = 11},
                new SurveyAnswerModel () { QCode = 10, AText = "No", ACode = 2, RuleType = 11},
                new SurveyAnswerModel () { QCode = 10, AText = "Could not observe", ACode = 5, RuleType = 11},

                new SurveyAnswerModel () { QCode = 11, AText = "Yes", ACode = 1, RuleType = 12},
                new SurveyAnswerModel () { QCode = 11, AText = "No", ACode = 2, RuleType = 12},
                new SurveyAnswerModel () { QCode = 11, AText = "Could not observe", ACode = 5, RuleType = 12},

                new SurveyAnswerModel () { QCode = 12, AText = "Yes", ACode = 1, RuleType = 13},
                new SurveyAnswerModel () { QCode = 12, AText = "No", ACode = 2, RuleType = 13},
                new SurveyAnswerModel () { QCode = 12, AText = "Could not observe", ACode = 5, RuleType = 13},

                new SurveyAnswerModel () { QCode = 13, AText = "", ACode = 1, RuleType = -1}
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
                        QCode = item.QCode,
                        ACode = item.ACode,
                        AText = item.AText,
                        RuleType = item.RuleType,
                        IsSelected = false,
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

