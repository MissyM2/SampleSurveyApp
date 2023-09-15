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
                new SurveyQuestionModel () { QCode=1, QCodeDesc = "Q1", QText = "Question 1: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=2, QCodeDesc  = "Q2", QText = "Question 2: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=3, QCodeDesc  = "Q3", QText = "Question 3: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=4, QCodeDesc  = "Q4", QText = "Question 4: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=5, QCodeDesc  = "Q5a", QText = "Question 5: Multiple Answers", QType = "MultipleAnswers"},
                new SurveyQuestionModel () { QCode=6, QCodeDesc  = "Q5b", QText = "Question 6: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=7, QCodeDesc  = "Q6a", QText = "Question 8: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=8, QCodeDesc  = "Q6b", QText = "Question 9: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=9, QCodeDesc  = "Q6c", QText = "Question 10: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=10, QCodeDesc  = "Q6d", QText = "Question 11: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=11, QCodeDesc  = "Q6e", QText  = "Question 12: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=12, QCodeDesc  = "Q6f", QText = "Question 13: Single Answer", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=13, QCodeDesc  = "Q7", QText = "Question 14: Text", QType = "Text"},
                new SurveyQuestionModel () { QCode=14, QCodeDesc  = "Q5c", QText = "Question 14", QType = "SingleAnswer"},
                new SurveyQuestionModel () { QCode=15, QCodeDesc  = "Q15", QText = "Question 14: Text", QType = "Text"}

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
                        QCode=item.QCode,
                        QCodeDesc = item.QCodeDesc,
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

