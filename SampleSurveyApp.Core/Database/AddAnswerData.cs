﻿using System;
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
                new SurveyAnswerModel () { QCode = 1, ACode = 1, AText = "Yes", RuleType = 2},
                new SurveyAnswerModel () { QCode = 1, ACode = 2, AText = "No", RuleType = -1},

                new SurveyAnswerModel () { QCode = 2, ACode = 1, AText = "Yes", RuleType = 3},
                new SurveyAnswerModel () { QCode = 2, ACode = 2, AText = "No", RuleType = -1},

                new SurveyAnswerModel () { QCode = 3, ACode = 1, AText = "Yes", RuleType = 4},
                new SurveyAnswerModel () { QCode = 3, ACode = 2, AText = "No", RuleType = 4},

                new SurveyAnswerModel () { QCode = 4, ACode = 1, AText = "SingleSelectionOption 1", RuleType = 5},
                new SurveyAnswerModel () { QCode = 4, ACode = 2, AText = "SingleSelectionOption 2", RuleType = -1},

                new SurveyAnswerModel () { QCode = 5, ACode = 1, AText = "MultipleSelectionOption 1", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 2, AText = "MultipleSelectionOption 2", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 3, AText = "MultipleSelectionOption 3", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 4, AText = "MultipleSelectionOption 4", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 5, AText = "MultipleSelectionOption 5", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 6,AText = "MultipleSelectionOption 6", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 7, AText = "MultipleSelectionOption 7", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 8, AText = "MultipleSelectionOption 8", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 9, AText = "MultipleSelectionOption 9", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 10, AText = "MultipleSelectionOption 10", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 11, AText = "MultipleSelectionOption 11", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 12, AText = "MultipleSelectionOption 12", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 13, AText = "MultipleSelectionOption 13", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 14,AText = "MultipleSelectionOption 14", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 15, AText = "MultipleSelectionOption 15", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 16, AText = "MultipleSelectionOption 16", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 17,AText = "MultipleSelectionOption 17", RuleType = -1},
                new SurveyAnswerModel () { QCode = 5, ACode = 18, AText = "MultipleSelectionOption 18", RuleType = -1},

                new SurveyAnswerModel () { QCode = 6, ACode = 1, AText = "None", RuleType = 14},
                new SurveyAnswerModel () { QCode = 6, ACode = 2, AText = "A little", RuleType = 14},
                new SurveyAnswerModel () { QCode = 6, ACode = 3, AText = "Some", RuleType = 14},
                new SurveyAnswerModel () { QCode = 6, ACode = 4, AText = "A lot", RuleType = 14},
                new SurveyAnswerModel () { QCode = 6, ACode = 5,AText = "Could not observe",  RuleType = 14},

                new SurveyAnswerModel () { QCode = 7, ACode = 1, AText = "Yes", RuleType = 8},
                new SurveyAnswerModel () { QCode = 7, ACode = 2, AText = "No", RuleType = 8},
                new SurveyAnswerModel () { QCode = 7, ACode = 5, AText = "Could not observe", RuleType = 8},
                new SurveyAnswerModel () { QCode = 8, ACode = 1, AText = "Yes", RuleType = 9},
                new SurveyAnswerModel () { QCode = 8, ACode = 2, AText = "No", RuleType = 9},

                new SurveyAnswerModel () { QCode = 8, ACode = 5, AText = "Could not observe", RuleType = 9},
                new SurveyAnswerModel () { QCode = 9, ACode = 1, AText = "Yes", RuleType = 10},

                new SurveyAnswerModel () { QCode = 9, ACode = 2, AText = "No", RuleType = 10},
                new SurveyAnswerModel () { QCode = 9, ACode = 5, AText = "Could not observe", RuleType = 10},

                new SurveyAnswerModel () { QCode = 10, ACode = 1, AText = "Yes", RuleType = 11},
                new SurveyAnswerModel () { QCode = 10, ACode = 2, AText = "No", RuleType = 11},
                new SurveyAnswerModel () { QCode = 10, ACode = 5, AText = "Could not observe", RuleType = 11},

                new SurveyAnswerModel () { QCode = 11, ACode = 1, AText = "Yes", RuleType = 12},
                new SurveyAnswerModel () { QCode = 11, ACode = 2, AText = "No", RuleType = 12},
                new SurveyAnswerModel () { QCode = 11, AText = "Could not observe", ACode = 5, RuleType = 12},

                new SurveyAnswerModel () { QCode = 12, ACode = 1, AText = "Yes", RuleType = 13},
                new SurveyAnswerModel () { QCode = 12, ACode = 2, AText = "No", RuleType = 13},
                new SurveyAnswerModel () { QCode = 12, ACode = 5, AText = "Could not observe", RuleType = 13},

                new SurveyAnswerModel () { QCode = 13, ACode = 1, AText = "", RuleType = -1}
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

