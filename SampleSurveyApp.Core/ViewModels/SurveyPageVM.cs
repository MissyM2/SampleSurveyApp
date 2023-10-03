using System;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SampleSurveyApp.Core.ViewModels.Base;
using SampleSurveyApp.Core.Services;
using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
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

        public LocalizationResourceManager LocalizationResourceManager => LocalizationResourceManager.Instance;

        [ObservableProperty]
        public SurveyModel newSurvey;

        [ObservableProperty]
        public SurveyModel insertedSurvey;

        [ObservableProperty]
        bool surveyIsSaved = false;

        [ObservableProperty]
        public SurveyResponseModel newResponse;

        [ObservableProperty]
        public SurveyResponseModel insertedResponse;

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


        #region Visibility/Enabled Properties

        [ObservableProperty]
        bool isVisibleMainInstructionLbl;

        [ObservableProperty]
        bool isVisibleTextInstructionLbl;

        [ObservableProperty]
        bool isVisibleSurveyHeader = false;

        [ObservableProperty]
        bool isVisibleSurveyStartButton = true;

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
        bool isWorkingLeftBtn = true;

        [ObservableProperty]
        bool isWorkingRightBtn = true;

        #endregion

        #region Orientation Props

        [ObservableProperty]
        double screenHeight;

        [ObservableProperty]
        double scrollViewScreenHeight;

        [ObservableProperty]
        double screenWidth;

        #endregion

        #region Label Props and other UI

        [ObservableProperty]
        string mainQuestionLbl;

        [ObservableProperty]
        string mainInstructionLbl;

        [ObservableProperty]
        string textInstructionLbl;

        [ObservableProperty]
        string screenNameLbl;

        [ObservableProperty]
        string leftBtnLbl;

        [ObservableProperty]
        string rightBtnLbl;

        [ObservableProperty]
        string saveSurveyLbl;

        [ObservableProperty]
        public string flyoutBehaviorStr = "Disabled";

        [ObservableProperty]
        ImageSource checkMarkImage;

        #endregion

        #region Field Props
        [ObservableProperty]
        string qText;

        [ObservableProperty]
        string qCode;

        [ObservableProperty]
        string aText;

        [ObservableProperty]
        bool isSelected;

        [ObservableProperty]
        string selectedItem;

        [ObservableProperty]
        string qType;

        [ObservableProperty]
        string navRule;

        [ObservableProperty]
        int id;

        [ObservableProperty]
        string surveyId;

        [ObservableProperty]
        string selectedLanguage;

        [ObservableProperty]
        int count;

        #endregion


        #region Text Answer

        public List<AnswerGroup> UserAnswerGroups { get; private set; } = new List<AnswerGroup>();
        public ObservableCollection<SurveyResponseModel> AnswerCollection { get; set; } = new();

        public ObservableCollection<object> SelectedAnswers { get; } = new();

        [ObservableProperty]
        int textLen = 0;

        [ObservableProperty]
        int minTextLen = 3;

        [ObservableProperty]
        int maxTextLen = 25;
        #endregion

        #region Flags

        [ObservableProperty]
        bool isPortrait = true;

        [ObservableProperty]
        bool isLandscape = false;

        [ObservableProperty]
        bool isAnswerReview = false;

        [ObservableProperty]
        bool isReadyToSave = false;

        [ObservableProperty]
        bool answerReviewIsNext;
        #endregion




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


        #region Initial Setup

        [RelayCommand]
        public async Task Init()
        {
            try
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
                ScreenNameLbl = AppResources.ScreenNameLblStart;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SurveyPageVM.cs:Init((: '{ex}'");
            }
            
            
        }

        #endregion


        #region Navigation

        [RelayCommand]
        public async Task Navigate(string direction)
        {
            try
            {
                if (direction == "Next")
                {
                    if (IsReadyToSave == true)
                    {
                        await SaveSurvey();
                    }
                    else
                    {

                        // request answer if empty
                        if (CurrentQuestion.NextQCode == -2)
                        {
                            if (CurrentQuestion.QType == "Text")
                            {
                                if (TextLen < MinTextLen)
                                {
                                    await _messageService.CustomAlert("Too short", "Entry must be, at least " + MinTextLen + " characters long.", "OK");
                                    return;
                                }

                                if (TextLen > MaxTextLen)
                                {
                                    await _messageService.CustomAlert("Too long", "Entry can be no more than " + MaxTextLen + " characters long.", "OK");
                                    return;
                                }
                                //await _messageService.CustomAlert(AppResources.NoAnswerSingleTextMsgTitle, AppResources.NoAnswerTextMsg + " " + TextLen, "OK");
                            }

                            if (CurrentQuestion.QType == "SingleAnswer")
                            {
                                await _messageService.CustomAlert(AppResources.NoAnswerSingleTextMsgTitle, AppResources.NoAnswerSingleMsg, "OK");
                                return;
                            }

                            if (CurrentQuestion.QType == "MultipleAnswers")
                            {
                                await _messageService.CustomAlert(AppResources.NoAnswerMultipleMsgTitle, AppResources.NoAnswerMultipleMsg, "OK");
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
                            GetAnswerOptionsForCurrentQuestion();

                            // set title view
                            SetTitleViewValuesOnOpen();
                        }

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
                        GetAnswerOptionsForCurrentQuestion();

                        // set title view
                        SetTitleViewValuesOnOpen();
                    }
                    else // all others
                    {
                        // set current navigation rule
                        CurrentNavRule = CurrentQuestion.PrevQCode;
                        NextCurrentQuestion = AllPossibleQuestionsCollection.FirstOrDefault(v => v.CurrQCode == CurrentNavRule);

                        // set CurrentQuestion
                        CurrentQuestion = NextCurrentQuestion;

                        // get answers for current question
                        GetAnswerOptionsForCurrentQuestion();

                        // set title view
                        SetTitleViewValuesOnOpen();
                    }
                }


                SetScreenValuesOnOpen();

            }
            catch(Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:Navigate: '{ex}'");
            }
        }

        private void GetAnswerOptionsForCurrentQuestion()
        {
            try
            {
                AnswerOptionsForCurrentQuestionCollection.Clear();
                foreach (var answer in answerSource)
                {
                    if (answer.CurrQCode == CurrentQuestion.CurrQCode)
                    {
                        answer.AText = AppResources.ResourceManager.GetString(answer.ATextLocal, CultureInfo.CurrentCulture);
                        AnswerOptionsForCurrentQuestionCollection.Add(answer);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"SurveyPageVM.cs:GetAnswerOptionsForCurrentQuestion((: '{ex}'");
            }

        }

        #endregion


        #region User Input: Single, Multiple or Text

        [RelayCommand]
        void SingleCVSelectionChanged() //Single Answer
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:SingleCVSelectionChanged: '{ex}'");
            }
           
        }

        [RelayCommand]
        void MultipleCVSelectionChanged()  // Multiple Answers
        {
            try
            {
                foreach (var item in SelectedAnswers)
                {
                    var currAnswer = item as SurveyAnswerModel;
                    currAnswer.IsSelected = true;

                    //Check to see if this is the last question
                    if (currAnswer.NavRule != -1)
                    {
                        CurrentQuestion.NextQCode = currAnswer.NavRule;
                        RightBtnLbl = AppResources.RightBtnLblNext;
                    }
                    else
                    {
                        CurrentQuestion.NextQCode = -1;
                        AnswerReviewIsNext = true;
                        RightBtnLbl = AppResources.RightBtnLblReview;
                    }
                }
                AnswerHasBeenSelected = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:MultipleCVSelectionChanged: '{ex}'");
            }
            
        }

        [RelayCommand]
        public void UserTextAnswerChanged()
        {
            try
            {
                TextLen = UserTextAnswer.Length;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:UserTextAnswerChanged: '{ex}'");
            }
            
        }

        #endregion


        #region UI Screen Values

        public void SetTitleViewValuesOnOpen()
        {
            try
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
                        ScreenNameLbl = AppResources.ResourceManager.GetString(CurrentQuestion.CurrQCodeDescLocal, CultureInfo.CurrentCulture);
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
                        RightBtnLbl = "Save Survey";
                        IsReadyToSave = true;
                        IsWorkingRightBtn = true;

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:SetTitleViewValuesOnOpen: '{ex}'");
            }
        }


        public void SetScreenValuesOnOpen()
        {
            try
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
                        MainQuestionLbl = AppResources.ResourceManager.GetString(CurrentQuestion.QTextLocal, CultureInfo.CurrentCulture);
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
                            IsVisibleMainInstructionLbl = false;
                            IsVisibleMainInstructionLbl = true;
                            IsVisibleTextInstructionLbl = true;
                        }
                    }
                    else  // must be review page
                    {
                        SaveSurveyLbl = AppResources.SaveSurveyBtnLbl;
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
            catch (Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:SetScreenValuesOnOpen: '{ex}'");
            }
            
        }

        #endregion

        
        #region Review Screen

        private void CreateUserResponsesCollection()
        {
            try
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
            catch(Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:CreateUserResponsesCollection: '{ex}'");
            }
           
        }

        public class AnswerGroup : List<SurveyAnswerModel>
        {
            public int CurrQCode { get; set; }
            public string QText { get; set; }
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
            try
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

                //get curr q
                CurrentQuestion = AllPossibleQuestionsCollection[0];
                CurrentQuestion.CurrQCodeDesc = AppResources.ResourceManager.GetString(CurrentQuestion.CurrQCodeDescLocal, CultureInfo.CurrentCulture);

                // get answers for curr q
                GetAnswerOptionsForCurrentQuestion();

                // update question in q collection
                var foundQ = AllPossibleQuestionsCollection.FirstOrDefault(x => x.CurrQCode.Equals(CurrentQuestion.CurrQCode));
                foundQ.IsSelected = true;
                foundQ.PrevQCode = 0;

                IsVisibleSurveyHeader = false;

                // set screen values based on properties in CurrentQuestion

                SetTitleViewValuesOnOpen();
                SetScreenValuesOnOpen();
                //Shell.Current.FlyoutIsPresented = false;
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:CreateSurvey: '{ex}'");
            }
            

        }

        [RelayCommand]
        public async Task SaveSurvey()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"SurveyPageVM.cs:SaveSurvey: '{ex}'");
            }
            

        }

        #endregion


    }
}

