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
            _surveyQuestionModelRepository=surveyQuestionModelRepository;

            SurveyQuestionList=new List<SurveyQuestionModel>()
            {
                new SurveyQuestionModel () { CurrQCode=01, CurrQCodeDesc="Q01", QText="Question 1: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=02, CurrQCodeDesc ="Q02", QText="Question 2: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=03, CurrQCodeDesc ="Q03", QText="Question 3: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=04, CurrQCodeDesc ="Q04", QText="Question 4: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=05, CurrQCodeDesc ="Q05", QText="Question 5: Multiple Answers", QType="MultipleAnswers"},
                new SurveyQuestionModel () { CurrQCode=06, CurrQCodeDesc ="Q06", QText="Question 6: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=07, CurrQCodeDesc ="Q07", QText="Question 7: Single Answer", QType="Singlenswer"},
                new SurveyQuestionModel () { CurrQCode=08, CurrQCodeDesc ="Q08", QText="Question 8: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=09, CurrQCodeDesc ="Q09", QText="Question 9: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=10, CurrQCodeDesc ="Q10", QText="Question 10: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=11, CurrQCodeDesc ="Q11", QText ="Question 11: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=12, CurrQCodeDesc ="Q12", QText="Question 12: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=13, CurrQCodeDesc ="Q13", QText="Question 13: Text", QType="Text"},
                new SurveyQuestionModel () { CurrQCode=14, CurrQCodeDesc ="Q14", QText="Question 14: Single Answer", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=15, CurrQCodeDesc ="Q15", QText="Question 15: Text", QType="Text"}

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
                        Id=item.Id,
                        QType=item.QType,
                        CurrQCode=item.CurrQCode,
                        CurrQCodeDesc=item.CurrQCodeDesc,
                        QText=item.QText,
                        PrevQCode=-2,
                        NextQCode=-2,
                        IsSelected=false
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

