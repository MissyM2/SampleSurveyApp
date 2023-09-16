using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace SampleSurveyApp.Maui.Controls;

public partial class AnswerOptionCardView : ContentView
{
	public AnswerOptionCardView()
	{
		InitializeComponent();
	}

    void OnNormalStateIsActiveChanged(object sender, EventArgs e)
    {
        StateTriggerBase stateTrigger = sender as StateTriggerBase;
        Debug.WriteLine($"Normal state active: {stateTrigger.IsActive}");
    }

    void OnSelectedStateIsActiveChanged(object sender, EventArgs e)
    {
        StateTriggerBase stateTrigger = sender as StateTriggerBase;
        Debug.WriteLine($"Selected state active: {stateTrigger.IsActive}");
    }


    //[RelayCommand]
    //private async void GoBack()
    //{
    //    Console.WriteLine("BackButtonClicked");



    //    // see if it is review page
    //    if (IsAnswerReview == true)
    //    {
    //        IsAnswerReview = false;
    //        // var foundQs = AllPossibleQuestionsCollection.Where(x => x.IsSelected.Equals(true));
    //        //CurrentQuestion = foundQs.Last(); if (CurrentQuestion.QType == "SingleAnswer" || CurrentQuestion.QType == "MultipleAnswers")
    //        CurrentQuestion = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode.Equals(CurrentQuestion.QCode));
    //        AnswerOptionsForCurrentQuestionCollection.Clear();
    //        foreach (var answer in answerSource)
    //        {
    //            if (answer.QCode == CurrentQuestion.QCode)
    //            {

    //                AnswerOptionsForCurrentQuestionCollection.Add(answer);
    //            }
    //        }
    //        if (CurrentQuestion.QType == "SingleAnswer" || CurrentQuestion.QType == "MultipleAnswers")
    //        {
    //            MakeSureAnswerHasBeenSelectedForCurrentQuestion();
    //        }
    //        else  // CurrentQuestion.QType must be Text
    //        {
    //            Debug.WriteLine("Get existing text for back button");
    //        }
    //    }
    //    else
    //    {
    //        //get new curr q from prev q
    //        CurrentQuestion = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode.Equals(CurrentQuestion.PrevQCode));
    //        // get answers for curr q
    //        AnswerOptionsForCurrentQuestionCollection.Clear();
    //        foreach (var answer in answerSource)
    //        {
    //            if (answer.QCode == CurrentQuestion.QCode)
    //            {

    //                AnswerOptionsForCurrentQuestionCollection.Add(answer);
    //            }
    //        }
    //        if (CurrentQuestion.QType == "SingleAnswer" || CurrentQuestion.QType == "MultipleAnswers")
    //        {
    //            MakeSureAnswerHasBeenSelectedForCurrentQuestion();
    //        }
    //        else  // CurrentQuestion.QType must be Text
    //        {
    //            Debug.WriteLine("Get existing text for back button");
    //        }
    //    }

    //    // set screen values based on properties in CurrentQuestion
    //    SetScreenValuesOnOpen();

    //}



}
