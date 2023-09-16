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
                new SurveyAnswerModel () { CurrQCode = 1, ACode = 1, AText = "Yes", NavRule = 2},
                new SurveyAnswerModel () { CurrQCode = 1, ACode = 2, AText = "No", NavRule = 2},

                new SurveyAnswerModel () { CurrQCode = 2, ACode = 1, AText = "Yes", NavRule = 3},
                new SurveyAnswerModel () { CurrQCode = 2, ACode = 2, AText = "No", NavRule = -1},

                new SurveyAnswerModel () { CurrQCode = 3, ACode = 1, AText = "Yes", NavRule = 4},
                new SurveyAnswerModel () { CurrQCode = 3, ACode = 2, AText = "No (Go to Text)", NavRule = 15},

                new SurveyAnswerModel () { CurrQCode = 4, ACode = 1, AText = "SingleSelectionOption 1", NavRule = 5},
                new SurveyAnswerModel () { CurrQCode = 4, ACode = 2, AText = "SingleSelectionOption 2", NavRule = -1},

                new SurveyAnswerModel () { CurrQCode = 5, ACode = 1, AText = "MultipleSelectionOption 1", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 2, AText = "MultipleSelectionOption 2", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 3, AText = "MultipleSelectionOption 3", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 4, AText = "MultipleSelectionOption 4", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 5, AText = "MultipleSelectionOption 5", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 6,AText = "MultipleSelectionOption 6", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 7, AText = "MultipleSelectionOption 7", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 8, AText = "MultipleSelectionOption 8", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 9, AText = "MultipleSelectionOption 9", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 10, AText = "MultipleSelectionOption 10", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 11, AText = "MultipleSelectionOption 11", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 12, AText = "MultipleSelectionOption 12", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 13, AText = "MultipleSelectionOption 13", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 14,AText = "MultipleSelectionOption 14", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 15, AText = "MultipleSelectionOption 15", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 16, AText = "MultipleSelectionOption 16", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 17,AText = "MultipleSelectionOption 17", NavRule = -1},
                new SurveyAnswerModel () { CurrQCode = 5, ACode = 18, AText = "MultipleSelectionOption 18", NavRule = -1},

                new SurveyAnswerModel () { CurrQCode = 6, ACode = 1, AText = "None", NavRule = 14},
                new SurveyAnswerModel () { CurrQCode = 6, ACode = 2, AText = "A little", NavRule = 14},
                new SurveyAnswerModel () { CurrQCode = 6, ACode = 3, AText = "Some", NavRule = 14},
                new SurveyAnswerModel () { CurrQCode = 6, ACode = 4, AText = "A lot", NavRule = 14},
                new SurveyAnswerModel () { CurrQCode = 6, ACode = 5,AText = "Could not observe",  NavRule = 14},

                new SurveyAnswerModel () { CurrQCode = 7, ACode = 1, AText = "Yes", NavRule = 8},
                new SurveyAnswerModel () { CurrQCode = 7, ACode = 2, AText = "No", NavRule = 8},
                new SurveyAnswerModel () { CurrQCode = 7, ACode = 5, AText = "Could not observe", NavRule = 8},
                new SurveyAnswerModel () { CurrQCode = 8, ACode = 1, AText = "Yes", NavRule = 9},
                new SurveyAnswerModel () { CurrQCode = 8, ACode = 2, AText = "No", NavRule = 9},

                new SurveyAnswerModel () { CurrQCode = 8, ACode = 5, AText = "Could not observe", NavRule = 9},
                new SurveyAnswerModel () { CurrQCode = 9, ACode = 1, AText = "Yes", NavRule = 10},

                new SurveyAnswerModel () { CurrQCode = 9, ACode = 2, AText = "No", NavRule = 10},
                new SurveyAnswerModel () { CurrQCode = 9, ACode = 5, AText = "Could not observe", NavRule = 10},

                new SurveyAnswerModel () { CurrQCode = 10, ACode = 1, AText = "Yes", NavRule = 11},
                new SurveyAnswerModel () { CurrQCode = 10, ACode = 2, AText = "No", NavRule = 11},
                new SurveyAnswerModel () { CurrQCode = 10, ACode = 5, AText = "Could not observe", NavRule = 11},

                new SurveyAnswerModel () { CurrQCode = 11, ACode = 1, AText = "Yes", NavRule = 12},
                new SurveyAnswerModel () { CurrQCode = 11, ACode = 2, AText = "No", NavRule = 12},
                new SurveyAnswerModel () { CurrQCode = 11, AText = "Could not observe", ACode = 5, NavRule = 12},

                new SurveyAnswerModel () { CurrQCode = 12, ACode = 1, AText = "Yes", NavRule = 13},
                new SurveyAnswerModel () { CurrQCode = 12, ACode = 2, AText = "No", NavRule = 13},
                new SurveyAnswerModel () { CurrQCode = 12, ACode = 5, AText = "Could not observe", NavRule = 13},

                new SurveyAnswerModel () { CurrQCode = 13, ACode = 1, AText = "", NavRule = -1},

                new SurveyAnswerModel () { CurrQCode = 15, ACode = 1, AText = "", NavRule = 3}
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
                        CurrQCode = item.CurrQCode,
                        ACode = item.ACode,
                        AText = item.AText,
                        NavRule = item.NavRule,
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

