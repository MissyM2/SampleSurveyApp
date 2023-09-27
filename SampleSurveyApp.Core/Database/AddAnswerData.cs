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
            _surveyAnswerModelRepository=surveyAnswerModelRepository;
            SurveyAnswerList=new List<SurveyAnswerModel>()
            {
                new SurveyAnswerModel () { CurrQCode=01, ACode=01, AText="Yes", ATextLocal="CurrQCode01ACode01AText", NavRule=2},
                new SurveyAnswerModel () { CurrQCode=01, ACode=02, AText="No", ATextLocal="CurrQCode01ACode02AText", NavRule=2},

                new SurveyAnswerModel () { CurrQCode=02, ACode=01, AText="Yes", ATextLocal="CurrQCode02ACode01AText", NavRule=3},
                new SurveyAnswerModel () { CurrQCode=02, ACode=02, AText="No (goes to Review)", ATextLocal="CurrQCode02ACode02AText", NavRule=-1},

                new SurveyAnswerModel () { CurrQCode=03, ACode=01, AText="Yes", ATextLocal="CurrQCode03ACode01AText", NavRule=4},
                new SurveyAnswerModel () { CurrQCode=03, ACode=02, AText="No (Goes to Text)", ATextLocal="CurrQCode03ACode02AText", NavRule=15},

                new SurveyAnswerModel () { CurrQCode=15, ACode=01, AText="", ATextLocal="CurrQCode15ACode01AText", NavRule=4},

                new SurveyAnswerModel () { CurrQCode=04, ACode=01, AText="Single Selection Option 1", ATextLocal="CurrQCode04ACode01AText", NavRule=5},
                new SurveyAnswerModel () { CurrQCode=04, ACode=02, AText="Single Selection Option 2 (Goes to Review)", ATextLocal="CurrQCode04ACode02AText", NavRule=-1},

                new SurveyAnswerModel () { CurrQCode=05, ACode=01, AText="Multiple Selection Option 1", ATextLocal="CurrQCode05ACode01AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=02, AText="Multiple Selection Option 2", ATextLocal="CurrQCode05ACode02AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=03, AText="Multiple Selection Option 3", ATextLocal="CurrQCode05ACode03AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=04, AText="Multiple Selection Option 4", ATextLocal="CurrQCode05ACode04AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=05, AText="Multiple Selection Option 5", ATextLocal="CurrQCode05ACode05AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=06, AText="Multiple Selection Option 6", ATextLocal="CurrQCode05ACode06AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=07, AText="Multiple Selection Option 7", ATextLocal="CurrQCode05ACode07AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=08, AText="Multiple Selection Option 8", ATextLocal="CurrQCode05ACode08AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=09, AText="Multiple Selection Option 9", ATextLocal="CurrQCode05ACode09AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=10, AText="Multiple Selection Option 10", ATextLocal="CurrQCode05ACode10AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=11, AText="Multiple Selection Option 11", ATextLocal="CurrQCode05ACode11AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=12, AText="Multiple Selection Option 12", ATextLocal="CurrQCode05ACode12AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=13, AText="Multiple Selection Option 13", ATextLocal="CurrQCode05ACode13AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=14, AText="Multiple Selection Option 14", ATextLocal="CurrQCode05ACode14AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=15, AText="Multiple Selection Option 15", ATextLocal="CurrQCode05ACode15AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=16, AText="Multiple Selection Option 16", ATextLocal="CurrQCode05ACode16AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=17, AText="Multiple Selection Option 17", ATextLocal="CurrQCode05ACode17AText", NavRule=-1},
                new SurveyAnswerModel () { CurrQCode=05, ACode=18, AText="Multiple Selection Option 18", ATextLocal="CurrQCode05ACode18AText", NavRule=-1},

                new SurveyAnswerModel () { CurrQCode=06, ACode=01, AText="None", ATextLocal="CurrQCode06ACode01AText", NavRule=14},
                new SurveyAnswerModel () { CurrQCode=06, ACode=02, AText="A little", ATextLocal="CurrQCode06ACode02AText", NavRule=14},
                new SurveyAnswerModel () { CurrQCode=06, ACode=03, AText="Some", ATextLocal="CurrQCode06ACode03AText", NavRule=14},
                new SurveyAnswerModel () { CurrQCode=06, ACode=04, AText="A lot", ATextLocal="CurrQCode06ACode04AText", NavRule=14},
                new SurveyAnswerModel () { CurrQCode=06, ACode=05, AText="Could not observe", ATextLocal="CurrQCode06ACode05AText",   NavRule=14},

                new SurveyAnswerModel () { CurrQCode=07, ACode=01, AText="Yes", ATextLocal="CurrQCode07ACode01AText", NavRule=8},
                new SurveyAnswerModel () { CurrQCode=07, ACode=02, AText="No", ATextLocal="CurrQCode07ACode02AText", NavRule=8},
                new SurveyAnswerModel () { CurrQCode=07, ACode=03, AText="Could not observe", ATextLocal="CurrQCode07ACode03AText", NavRule=8},
                
                new SurveyAnswerModel () { CurrQCode=08, ACode=01, AText="Yes", ATextLocal="CurrQCode08ACode01AText", NavRule=9},
                new SurveyAnswerModel () { CurrQCode=08, ACode=02, AText="No", ATextLocal="CurrQCode08ACode02AText", NavRule=9},
                new SurveyAnswerModel () { CurrQCode=08, ACode=03, AText="Could not observe", ATextLocal="CurrQCode08ACode03AText", NavRule=9},

                new SurveyAnswerModel () { CurrQCode=09, ACode=01, AText="Yes", ATextLocal="CurrQCode09ACode01AText", NavRule=10},
                new SurveyAnswerModel () { CurrQCode=09, ACode=02, AText="No", ATextLocal="CurrQCode09ACode02AText", NavRule=10},
                new SurveyAnswerModel () { CurrQCode=09, ACode=03, AText="Could not observe", ATextLocal="CurrQCode09ACode03AText", NavRule=10},

                new SurveyAnswerModel () { CurrQCode=10, ACode=01, AText="Yes", ATextLocal="CurrQCode10ACode01AText", NavRule=11},
                new SurveyAnswerModel () { CurrQCode=10, ACode=02, AText="No", ATextLocal="CurrQCode10ACode02AText", NavRule=11},
                new SurveyAnswerModel () { CurrQCode=10, ACode=03, AText="Could not observe", ATextLocal="CurrQCode10ACode03AText", NavRule=11},

                new SurveyAnswerModel () { CurrQCode=11, ACode=01, AText="Yes", ATextLocal="CurrQCode11ACode01AText", NavRule=12},
                new SurveyAnswerModel () { CurrQCode=11, ACode=02, AText="No", ATextLocal="CurrQCode11ACode02AText", NavRule=12},
                new SurveyAnswerModel () { CurrQCode=11, ACode=03, AText="Could not observe", ATextLocal="CurrQCode11ACode03AText", NavRule=12},

                new SurveyAnswerModel () { CurrQCode=12, ACode=01, AText="Yes", ATextLocal="CurrQCod12ACode01AText", NavRule=13},
                new SurveyAnswerModel () { CurrQCode=12, ACode=02, AText="No", ATextLocal="CurrQCode12ACode02AText", NavRule=13},
                new SurveyAnswerModel () { CurrQCode=12, ACode=03, AText="Could not observe", ATextLocal="CurrQCode12ACode03AText", NavRule=13},

                new SurveyAnswerModel () { CurrQCode=13, ACode=01, AText="Red", ATextLocal="CurrQCode13ACode01AText", NavRule=14},
                new SurveyAnswerModel () { CurrQCode=13, ACode=01, AText="Blue", ATextLocal="CurrQCode13ACode02AText", NavRule=14},


                new SurveyAnswerModel () { CurrQCode=14, ACode=01, AText="", ATextLocal="CurrQCode14ACode01AText", NavRule=-1}

                
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
                        Id=item.Id,
                        CurrQCode=item.CurrQCode,
                        ACode=item.ACode,
                        AText=item.AText,
                        ATextLocal=item.ATextLocal,
                        NavRule=item.NavRule,
                        IsSelected=false,
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

