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
                new SurveyQuestionModel () { CurrQCode=01, CurrQCodeDesc="Q01", CurrQCodeDescLocal="Q01CurrQCodeDescQ", QText="Question 1: Single Answer", QTextLocal="Q01QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=02, CurrQCodeDesc="Q02", CurrQCodeDescLocal="Q02CurrCodeDesc", QText="Question 2: Single Answer", QTextLocal="Q02QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=03, CurrQCodeDesc="Q03", CurrQCodeDescLocal="Q03CurrQCodeDesc", QText="Question 3: Single Answer", QTextLocal="Q03QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=04, CurrQCodeDesc="Q04", CurrQCodeDescLocal="Q04CurrQCodeDesc", QText="Question 4: Single Answer", QTextLocal="Q04QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=05, CurrQCodeDesc="Q05", CurrQCodeDescLocal="Q05CurrQCodeDesc", QText="Question 5: Multiple Answers", QTextLocal="Q05QText", QType="MultipleAnswers"},
                new SurveyQuestionModel () { CurrQCode=06, CurrQCodeDesc="Q06", CurrQCodeDescLocal="Q06CurrQCodeDesc", QText="Question 6: Single Answer", QTextLocal="Q06QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=07, CurrQCodeDesc="Q07", CurrQCodeDescLocal="Q07CurrQCodeDesc", QText="Question 7: Single Answer", QTextLocal="Q07QText", QType="Singlenswer"},
                new SurveyQuestionModel () { CurrQCode=08, CurrQCodeDesc="Q08", CurrQCodeDescLocal="Q08CurrQCodeDesc", QText="Question 8: Single Answer", QTextLocal="Q08QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=09, CurrQCodeDesc="Q09", CurrQCodeDescLocal="Q09CurrQCodeDesc", QText="Question 9: Single Answer", QTextLocal="Q09QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=10, CurrQCodeDesc="Q10", CurrQCodeDescLocal="Q10CurrQCodeDesc", QText="Question 10: Single Answer", QTextLocal="Q10QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=11, CurrQCodeDesc="Q11", CurrQCodeDescLocal="Q11CurrQCodeDesc", QText="Question 11: Single Answer", QTextLocal="Q11QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=12, CurrQCodeDesc="Q12", CurrQCodeDescLocal="Q12CurrQCodeDesc", QText="Question 12: Single Answer", QTextLocal="Q12QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=13, CurrQCodeDesc="Q13", CurrQCodeDescLocal="Q13CurrQCodeDesc", QText="Question 13: Text", QTextLocal="Q13QText", QType="Text"},
                new SurveyQuestionModel () { CurrQCode=14, CurrQCodeDesc="Q14", CurrQCodeDescLocal="Q14CurrQCodeDesc", QText="Question 14: Single Answer", QTextLocal="Q14QText", QType="SingleAnswer"},
                new SurveyQuestionModel () { CurrQCode=15, CurrQCodeDesc="Q15", CurrQCodeDescLocal="Q15CurrQCodeDesc", QText="Question 15: Text", QTextLocal="Q15QText", QType="Text"}

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
                        CurrQCodeDescLocal=item.CurrQCodeDescLocal,
                        QText=item.QText,
                        QTextLocal=item.QTextLocal,
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

