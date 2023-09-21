using System;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SampleSurveyApp.Core.ViewModels.Base;
using SampleSurveyApp.Core.Services;
using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using System.Windows.Input;
using SampleSurveyApp.Core.Localization;

namespace SampleSurveyApp.Core.ViewModels
{
    [QueryProperty("Id", "Id")]
    public partial class SurveyPageVM : BaseVM
    {

        #region Dependency Injection

        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        private readonly IUserPreferences _userPreferences;

        private readonly IAsyncRepository<SurveyQuestionModel> _surveyQuestionModelRepository;
        private readonly IAsyncRepository<SurveyAnswerModel> _surveyAnswerModelRepository;
        private readonly IAsyncRepository<SurveyModel> _surveyModelRepository;
        private readonly IAsyncRepository<SurveyResponseModel> _surveyResponseModelRepository;

        #endregion

        #region Question and Answer Options Lists and Observable Collections

        public List<SurveyQuestionModel> questionSource { get; set; } = new();
        public List<SurveyAnswerModel> answerSource { get; set; } = new();

        public ObservableCollection<SurveyQuestionModel> AllPossibleQuestionsCollection { get; set; }
        public ObservableCollection<SurveyAnswerModel> AllPossibleAnswerOptionsCollection { get; set; }

        #endregion

        #region Current Question and Current Answers

        // these are the answers either selected or input by the users
        [ObservableProperty]
        public SurveyAnswerModel currentlySelectedAnswer;

        public List<SurveyAnswerModel> CurrentlySelectedAnswers { get; set; } = new();

        [ObservableProperty]
        public string userTextAnswer;

        [ObservableProperty]
        SurveyQuestionModel currentQuestion;

        [ObservableProperty]
        SurveyQuestionModel nextCurrentQuestion;

        [ObservableProperty]
        int currentNavRule;

        [ObservableProperty]
        bool questionHasBeenAnswered = false;

        [ObservableProperty]
        bool answerHasBeenSelected = false;

        public IList<SurveyAnswerModel> AnswerOptionsForCurrentQuestionCollection { get; set; }


        #endregion


        #region UI properties

        [ObservableProperty]
        ImageSource checkMarkImage;

        [ObservableProperty]
        string mainQuestionLbl;

        [ObservableProperty]
        string mainInstructionLbl;

        [ObservableProperty]
        bool isVisibleMainInstructionLbl;

        [ObservableProperty]
        string textInstructionLbl = "The maximum character allowed is ";

        [ObservableProperty]
        bool isVisibleTextInstructionLbl;

        [ObservableProperty]
        int count;

        [ObservableProperty]
        string screenNameLbl = "Survey Start Page";

        [ObservableProperty]
        string leftBtnLbl;

        [ObservableProperty]
        string rightBtnLbl;

        [ObservableProperty]
        bool isVisibleQTypeText;

        [ObservableProperty]
        bool isVisibleRuleTypeSingle;

        [ObservableProperty]
        bool isVisibleRuleTypeMultiple;

        [ObservableProperty]
        bool isVisibleAnswerReview;

        [ObservableProperty]
        bool isVisibleThankYouText;

       [ObservableProperty]
        bool isAnswerReview = false;

        [ObservableProperty]
        bool answerReviewIsNext;

        [ObservableProperty]
        bool isVisibleSurveyHeader = false;

        [ObservableProperty]
        bool isVisibleSurveyStartButton = true;

        [ObservableProperty]
        bool isWorkingLeftBtn = true;

        [ObservableProperty]
        bool isWorkingRightBtn = true;

        [ObservableProperty]
        string qText;

        [ObservableProperty]
        string qCode;

        [ObservableProperty]
        string aText;

        [ObservableProperty]
        bool isSelected;

        #endregion

        [ObservableProperty]
        string selectedItem;

        [ObservableProperty]
        int id;

        [ObservableProperty]
        string surveyId;

        [ObservableProperty]
        int textLen = 0;

        [ObservableProperty]
        int minTextLen = 3;

        [ObservableProperty]
        int maxTextLen = 25;

        [ObservableProperty]
        string qType;

        [ObservableProperty]
        string navRule;

        public List<AnswerGroup> UserAnswerGroups { get; private set; } = new List<AnswerGroup>();
        public ObservableCollection<SurveyResponseModel> AnswerCollection { get; set; } = new();

        [ObservableProperty]
        public SurveyModel newSurvey;

        [ObservableProperty]
        public SurveyModel insertedSurvey;

        [ObservableProperty]
        string saveSurveyLbl = AppResources.SaveSurveyBtnLbl;

        [ObservableProperty]
        bool surveyIsSaved = false;

        [ObservableProperty]
        public SurveyResponseModel newResponse;

        [ObservableProperty]
        public SurveyResponseModel insertedResponse;




        public SurveyPageVM(
            INavigationService navigationService,
            IMessageService messageService,
            IUserPreferences userPreferences,
            IAsyncRepository<SurveyQuestionModel> surveyQuestionModelRepository,
            IAsyncRepository<SurveyAnswerModel> surveyAnswerModelRepository,
            IAsyncRepository<SurveyModel> surveyModelRepository,
            IAsyncRepository<SurveyResponseModel> surveyResponseModelRepository)
        {
            _navigationService = navigationService;
            _messageService = messageService;
            _userPreferences = userPreferences;
            _surveyQuestionModelRepository = surveyQuestionModelRepository;
            _surveyAnswerModelRepository = surveyAnswerModelRepository;
            _surveyModelRepository = surveyModelRepository;
            _surveyResponseModelRepository = surveyResponseModelRepository;

            questionSource = new List<SurveyQuestionModel>();

            AllPossibleQuestionsCollection = new ObservableCollection<SurveyQuestionModel>();

            AllPossibleAnswerOptionsCollection = new ObservableCollection<SurveyAnswerModel>();
            AnswerOptionsForCurrentQuestionCollection = new ObservableCollection<SurveyAnswerModel>();
            CheckMarkImage = ImageSource.FromResource("SampleSurveyApp.Maui.Resources.Images.check.png", typeof(SurveyPageVM).GetTypeInfo().Assembly);

        }


       

        [RelayCommand]
        public async Task Init()
        {
            await _surveyQuestionModelRepository.DeleteAllAsync();
            var aqd = new AddQuestionData(_surveyQuestionModelRepository);
            await aqd.AddQuestionsAsync();

            await _surveyAnswerModelRepository.DeleteAllAsync();

            var aad = new AddAnswerData(_surveyAnswerModelRepository);
            await aad.AddAnswersAsync();


            if (answerSource.Any()) answerSource.Clear();
            answerSource = await _surveyAnswerModelRepository.GetAllAsync();

            foreach (var answer in answerSource)
            {
                AllPossibleAnswerOptionsCollection.Add(answer);
            }

            // get all possible questions and answers
            if (questionSource.Any()) questionSource.Clear();
            questionSource = await _surveyQuestionModelRepository.GetAllAsync();

            foreach (var question in questionSource)
            {
                AllPossibleQuestionsCollection.Add(question);
            }


            //get curr q
            CurrentQuestion = AllPossibleQuestionsCollection[0];

            // get answers for curr q
            AnswerOptionsForCurrentQuestionCollection.Clear();
            foreach (var answer in answerSource)
            {
                if (answer.CurrQCode == CurrentQuestion.CurrQCode)
                {
                    AnswerOptionsForCurrentQuestionCollection.Add(answer);
                }
            }
            // update question in q collection
            var foundQ = AllPossibleQuestionsCollection.FirstOrDefault(x => x.CurrQCode.Equals(CurrentQuestion.CurrQCode));
            foundQ.IsSelected = true;
            foundQ.PrevQCode = 0;

            IsVisibleSurveyHeader = false;

            // set screen values based on properties in CurrentQuestion
            SetTitleViewValuesOnOpen();
            SetScreenValuesOnOpen();

        }


        #region Navigation

        [RelayCommand]
        public async Task Navigate(string direction)
        {
            Console.WriteLine("NavigateClicked");
            //CurrentlySelectedAnswer = null;

            if (direction == "Next")
            {
                
                // request answer if empty

                if (CurrentQuestion.NextQCode == -2)
                {
                    if (CurrentQuestion.QType == "Text" && TextLen < 3)
                    {
                        Debug.WriteLine("no text entered");
                        await _messageService.CustomAlert("No Answer", "Please enter your answer, greater than 3 chars. You added " + TextLen, "OK");
                        return;
                    }

                    if (CurrentQuestion.QType == "SingleAnswer")
                    {
                        Debug.WriteLine("CurrentQuestion.Qtype ==SingleAnswer: no answer selected");
                        await _messageService.CustomAlert("No Answer", "Please make a choice.", "OK");
                        return;
                    }

                    if (CurrentQuestion.QType == "MultipleAnswers")
                    {
                        Debug.WriteLine("CurrentQuestion.QType == MultipleAnswers: no answer selected");
                        await _messageService.CustomAlert("Answer", "Please make your selection(s).", "OK");
                        return;
                    }
                    
                }

                // multiple choice and multiple selection answers are chosen before the Next is tapped
                if (CurrentQuestion.QType == "Text")
                {
                    //  there is only one answer option for a text q
                    CurrentlySelectedAnswer = AllPossibleAnswerOptionsCollection.FirstOrDefault(x => x.CurrQCode == CurrentQuestion.CurrQCode);
                }


                // set current nav rule
                if (CurrentQuestion.NextQCode == -1)  // next screen is review
                {
                    CurrentNavRule = -1;
                }

                if (CurrentNavRule == -1) // any question with a NavRule of -1, meaning the next screen is the AnswerReview
                {
                    // set current nav rule
                    if (CurrentQuestion.NextQCode == -2)
                    {
                        CurrentNavRule = -1;
                    }

                    // set 
                    IsAnswerReview = true;
                    
                    CurrentQuestion.NextQCode = -1;
                    CreateUserResponsesCollection();

                    // set title view
                    SetTitleViewValuesOnOpen();

                    // set screen values based on properties in CurrentQuestion
                    SetScreenValuesOnOpen();

                }
                else  // all questions with a NavRule other than -1, meaning the next screen is another question
                {
                    // set current nav rule
                    CurrentNavRule = CurrentQuestion.NextQCode;
                    NextCurrentQuestion = AllPossibleQuestionsCollection.FirstOrDefault(v => v.CurrQCode == CurrentNavRule);

                    // set prevqcode on CurrentQuestion
                    NextCurrentQuestion.PrevQCode = CurrentQuestion.CurrQCode;

                    // get new current question
                    CurrentQuestion = NextCurrentQuestion;
                    CurrentQuestion.IsSelected = true;


                    // get answers for current question
                    AnswerOptionsForCurrentQuestionCollection.Clear();
                    foreach (var i in AllPossibleAnswerOptionsCollection)
                    {
                        if (i.CurrQCode == CurrentQuestion.CurrQCode)
                        {
                            AnswerOptionsForCurrentQuestionCollection.Add(i);
                        }
                    }

                    // set title view
                    SetTitleViewValuesOnOpen();
                }


            }
            else // direction == "Prev"
            {
                // set current nav rule
                if (IsAnswerReview == true)
                {
                    if (CurrentQuestion.NextQCode == -1)
                    {
                        CurrentNavRule = -1;
                    }
                }

                IsAnswerReview = false;
                
                if (CurrentNavRule == -1)  // first question from review
                {
                    // set current navigation rule
                    CurrentNavRule = CurrentQuestion.CurrQCode;
                    
                    // get answers for current question
                    AnswerOptionsForCurrentQuestionCollection.Clear();
                    foreach (var i in AllPossibleAnswerOptionsCollection)
                    {
                        if (i.CurrQCode == CurrentQuestion.CurrQCode)
                        {
                            AnswerOptionsForCurrentQuestionCollection.Add(i);
                        }
                    }

                    // set title view
                    SetTitleViewValuesOnOpen();
                }
                else // all others
                {
                    // set current navigation rule
                    CurrentNavRule = CurrentQuestion.PrevQCode;
                    NextCurrentQuestion = AllPossibleQuestionsCollection.FirstOrDefault(v => v.CurrQCode == CurrentNavRule);

                    CurrentQuestion = NextCurrentQuestion;
                    // CurrentQuestion.IsSelected = true;


                    // get answers for current question
                    AnswerOptionsForCurrentQuestionCollection.Clear();
                    foreach (var i in AllPossibleAnswerOptionsCollection)
                    {
                        if (i.CurrQCode == CurrentQuestion.CurrQCode)
                        {
                            AnswerOptionsForCurrentQuestionCollection.Add(i);
                        }
                    }

                    // set title view
                    SetTitleViewValuesOnOpen();
                }
            }


            SetScreenValuesOnOpen();

        }

        #endregion


        #region User Input

        [RelayCommand]
        public async Task UserTextAnswerChanged()
        {

            Debug.WriteLine("User Text Answer");
            TextLen = UserTextAnswer.Length;
            if (TextLen == 0)
            {
                Debug.WriteLine("User has added 0 character." + TextLen);

            }
            else if (TextLen == 1)
            {
                Debug.WriteLine("User has added 1 character." + TextLen);
            }
            else
            {
                Debug.WriteLine("more than 1 character" + TextLen);
            }



        }

        public ICommand MultipleSelectionCommand => new Command<IList<object>>((obj) =>
        {
            List<SurveyAnswerModel> temp = new List<SurveyAnswerModel>();

            foreach (var item in obj)
            {
                var selectedItems = item as SurveyAnswerModel;
                selectedItems.IsSelected = true;

                temp.Add(selectedItems);

                // Check to see if this is the last question
                if (selectedItems.NavRule != -1)
                {
                    CurrentQuestion.NextQCode = selectedItems.NavRule;
                    //AnswerReviewIsNext = false;
                    RightBtnLbl = AppResources.RightBtnLblNext;
                }
                else
                {
                    CurrentQuestion.NextQCode = -1;
                    AnswerReviewIsNext = true;
                    RightBtnLbl = AppResources.RightBtnLblReview;
                }

            }
            CurrentlySelectedAnswers.Clear();
            foreach (var item in temp)
            {
                CurrentlySelectedAnswers.Add(item);
                CurrentNavRule = item.NavRule;

            }
            AnswerHasBeenSelected = true;
        });

        [RelayCommand]
        public async Task AnswerSelected() //Single Answer
        {
            
            var selectedAnswer = AnswerOptionsForCurrentQuestionCollection.FirstOrDefault(x => x.CurrQCode == CurrentlySelectedAnswer.CurrQCode && x.ACode == CurrentlySelectedAnswer.ACode);
            selectedAnswer.IsSelected = true;
            if (CurrentQuestion.QType == "Text")
            {
                selectedAnswer.AText = UserTextAnswer;
            }

            if (CurrentQuestion.QType != "Text")
            {
                var otherAnswer = AnswerOptionsForCurrentQuestionCollection.FirstOrDefault(x => x.CurrQCode == CurrentlySelectedAnswer.CurrQCode && x.ACode != CurrentlySelectedAnswer.ACode);
                otherAnswer.IsSelected = false;
            }

            CurrentNavRule = selectedAnswer.NavRule;
            

            if (CurrentlySelectedAnswer.NavRule != -1)
            {
                CurrentQuestion.NextQCode = CurrentlySelectedAnswer.NavRule;
                RightBtnLbl = AppResources.RightBtnLblNext;
            }
            else
            {
                CurrentQuestion.NextQCode = -1;
                AnswerReviewIsNext = true;
                RightBtnLbl = AppResources.RightBtnLblReview;
            }

            AnswerHasBeenSelected = true;
        }

        #endregion


        #region UI Screen Values

        public void SetTitleViewValuesOnOpen()
        {
            if (SurveyIsSaved == true)
            {
                LeftBtnLbl = "";
                IsWorkingLeftBtn = false;
                RightBtnLbl = "";
                IsWorkingRightBtn = false;
                ScreenNameLbl = AppResources.ScreenNameLblEndOfSurvey;
            }
            else
            {
                if (IsAnswerReview == false)
                {
                    IsWorkingRightBtn = true;
                    ScreenNameLbl = CurrentQuestion.CurrQCodeDesc;
                    if (CurrentQuestion.PrevQCode == 0)
                    {
                        LeftBtnLbl = "";
                        IsWorkingLeftBtn = false;
                    }
                    else
                    {
                        LeftBtnLbl = AppResources.LeftBtnLblPrev;
                        IsWorkingLeftBtn = true;
                    }

                    if (CurrentQuestion.NextQCode == -1)
                    {
                        RightBtnLbl = AppResources.RightBtnLblReview;
                    }
                    else
                    {
                        RightBtnLbl = AppResources.RightBtnLblNext;
                    }
                }
                else
                {
                    ScreenNameLbl = AppResources.ScreenNameLblReview;
                    LeftBtnLbl = AppResources.LeftBtnLblPrev;
                    RightBtnLbl = "";
                    IsWorkingRightBtn = false;

                }
            }
        }


        public void SetScreenValuesOnOpen()
        {
            SurveyId = InsertedSurvey.Suid.ToString();
            IsVisibleSurveyStartButton = false;

            if (SurveyIsSaved == true)
            {
                IsVisibleSurveyHeader = false;
                IsVisibleRuleTypeSingle = false;
                IsVisibleRuleTypeMultiple = false;
                IsVisibleAnswerReview = false;
                IsVisibleQTypeText = false;
                IsVisibleThankYouText = true;
            }
            else
            {
                IsVisibleSurveyHeader = true;

                if (IsAnswerReview == false)
                {
                    IsVisibleMainInstructionLbl = false;
                    IsVisibleTextInstructionLbl = false;
                    MainQuestionLbl = CurrentQuestion.QText;
                    IsVisibleRuleTypeSingle = true;
                    IsVisibleRuleTypeMultiple = false;
                    IsVisibleQTypeText = false;
                    IsVisibleAnswerReview = false;
                    IsVisibleThankYouText = false;

                    if (CurrentQuestion.QType == "SingleAnswer")     // CurrentQuestion.QType must be SingleAnswer
                    {
                        IsVisibleMainInstructionLbl = true;
                        IsVisibleTextInstructionLbl = false;
                        IsVisibleRuleTypeSingle = true;
                        IsVisibleRuleTypeMultiple = false;
                        IsVisibleQTypeText = false;
                        IsVisibleAnswerReview = false;
                        IsVisibleThankYouText = false;
                        MainInstructionLbl = AppResources.MainInstructionLblSingleAnswer;
                        IsVisibleMainInstructionLbl = true;
                        IsVisibleTextInstructionLbl = false;
                    }
                    else if (CurrentQuestion.QType == "MultipleAnswers")     // CurrentQuestion.QType must be MultipleAnswers
                    {
                        IsVisibleMainInstructionLbl = false;
                        IsVisibleTextInstructionLbl = false;
                        IsVisibleRuleTypeSingle = false;
                        IsVisibleRuleTypeMultiple = true;
                        IsVisibleQTypeText = false;
                        IsVisibleAnswerReview = false;
                        IsVisibleThankYouText = false;
                        MainInstructionLbl = AppResources.MainInstructionLblMultipleAnswers;
                        IsVisibleMainInstructionLbl = true;
                        IsVisibleTextInstructionLbl = false;
                    }
                    else // CurrentQuestion.QType must be Text
                    {
                        IsVisibleMainInstructionLbl = false;
                        IsVisibleTextInstructionLbl = true;
                        IsVisibleRuleTypeSingle = false;
                        IsVisibleRuleTypeMultiple = false;
                        IsVisibleAnswerReview = false;
                        IsVisibleQTypeText = true;
                        IsVisibleThankYouText = false;
                        IsVisibleMainInstructionLbl=false;
                        IsVisibleMainInstructionLbl = true;
                        IsVisibleTextInstructionLbl = true;
                    }
                }
                else
                {
                    MainQuestionLbl = AppResources.MainQuestionLblReview;
                    MainInstructionLbl = AppResources.MainInstructionLblReview;
                    IsVisibleMainInstructionLbl = true;
                    IsVisibleTextInstructionLbl = false;
                    IsVisibleTextInstructionLbl = false;
                    IsVisibleRuleTypeSingle = false;
                    IsVisibleRuleTypeMultiple = false;
                    IsVisibleAnswerReview = true;
                    IsVisibleQTypeText = false;
                    IsVisibleThankYouText = false;

                }
            }
        }

        #endregion

        
        #region Review Screen

        private void CreateUserResponsesCollection()
        {
            UserAnswerGroups.Clear();
            var dict = AllPossibleAnswerOptionsCollection.Where(x => x.IsSelected.Equals(true)).GroupBy(o => o.CurrQCode)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (KeyValuePair<int, List<SurveyAnswerModel>> item in dict)
            {
                        
                var q = AllPossibleQuestionsCollection.FirstOrDefault(x => x.CurrQCode == item.Key);
                UserAnswerGroups.Add(new AnswerGroup(item.Key, q.QText, new List<SurveyAnswerModel>(item.Value)));
            }
        }

        public class AnswerGroup : List<SurveyAnswerModel>
        {
            public int CurrQCode { get; set; }
            public string QText { get; set; }

            //public AnswerGroup(string qCode, string qText, List<SurveyResponseModel> userResponses) : base(userResponses)
            public AnswerGroup(int qCode, string qText, List<SurveyAnswerModel> userResponses) : base(userResponses)
            {
                CurrQCode = qCode;
                QText = qText;
            }
        }

        #endregion


        #region CRUD Operations

        [RelayCommand]
        public async Task CreateSurvey()
        {
            if (IsBusy) return;

            IsBusy = true;
            NewSurvey = new SurveyModel();
            NewSurvey.SurveyDate = DateTime.Now;
            NewSurvey.SurveyStatus = "I";
            NewSurvey.SyncStatus = "I";
            NewSurvey.Suid = Guid.NewGuid(); ;


            // insert a new record
            await _surveyModelRepository.InsertAsync(NewSurvey);
            InsertedSurvey = NewSurvey;
            await Init();
            IsBusy = false;

        }

        [RelayCommand]
        public async Task SaveSurvey()
        {
            // save survey responses
            var answerList = AllPossibleAnswerOptionsCollection.Where(x => x.IsSelected.Equals(true));
            foreach (var item in answerList)
            {

                NewResponse = new SurveyResponseModel();
                NewResponse.SurveyId = InsertedSurvey.Suid;
                NewResponse.CurrQCode = item.CurrQCode;
                var founditem = AllPossibleQuestionsCollection.FirstOrDefault(x => x.CurrQCode == item.CurrQCode);
                NewResponse.QText = founditem.QText;
                NewResponse.ACode = item.ACode;
                NewResponse.AText = item.AText;

                await _surveyResponseModelRepository.InsertAsync(NewResponse);
            }

            SurveyIsSaved = true;

            SetTitleViewValuesOnOpen();
            SetScreenValuesOnOpen();

        }

        #endregion

    }
}

