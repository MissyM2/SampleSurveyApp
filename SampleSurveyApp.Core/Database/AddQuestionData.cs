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
                new SurveyQuestionModel () { CurrQCode=1, CurrQCodeDesc = "Q1", QText = "Question 1: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=2, CurrQCodeDesc  = "Q2", QText = "Question 2: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=3, CurrQCodeDesc  = "Q3", QText = "Question 3: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=4, CurrQCodeDesc  = "Q4", QText = "Question 4: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=5, CurrQCodeDesc  = "Q5a", QText = "Question 5: Multiple Answers", QType = "MultipleAnswers"},
                new SurveyQuestionModel () { CurrQCode=6, CurrQCodeDesc  = "Q5b", QText = "Question 6: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=7, CurrQCodeDesc  = "Q6a", QText = "Question 8: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=8, CurrQCodeDesc  = "Q6b", QText = "Question 9: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=9, CurrQCodeDesc  = "Q6c", QText = "Question 10: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=10, CurrQCodeDesc  = "Q6d", QText = "Question 11: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=11, CurrQCodeDesc  = "Q6e", QText  = "Question 12: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=12, CurrQCodeDesc  = "Q6f", QText = "Question 13: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=13, CurrQCodeDesc  = "Q7", QText = "Question 14: Text", QType = "Text"},
                new SurveyQuestionModel () { CurrQCode=14, CurrQCodeDesc  = "Q5c", QText = "Question 14", QType = "SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=15, CurrQCodeDesc  = "Q15", QText = "Question 14: Text", QType = "Text"}

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
                        QType = item.QType,
                        CurrQCode=item.CurrQCode,
                        CurrQCodeDesc = item.CurrQCodeDesc,
                        QText = item.QText,
                        PrevQCode = 0,
                        NextQCode = 0,
                        IsSelected = false
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

