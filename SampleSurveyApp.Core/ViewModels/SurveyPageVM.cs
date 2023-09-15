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

        #region Q and A lists

        public List<SurveyQuestionModel> questionSource { get; set; } = new();
        public List<SurveyAnswerModel> answerSource { get; set; } = new();

        public ObservableCollection<SurveyQuestionModel> AllPossibleQuestionsCollection { get; set; }

        public ObservableCollection<SurveyAnswerModel> AllPossibleAnswerOptionsCollection { get; set; }

        #endregion

        #region Current Question and Current Answers

        // these two properties (1 object, 1 list of objects are for answers selected by user - either single or multiselecct
        [ObservableProperty]
        public SurveyAnswerModel userSelectedAnswer;

        public List<SurveyAnswerModel> UserSelectedAnswers { get; set; } = new();

        [ObservableProperty]
        public string userTextAnswer;

        [ObservableProperty]
        SurveyQuestionModel currentQuestion;

        public IList<SurveyAnswerModel> AnswerOptionsForCurrentQuestionCollection { get; set; }


        [ObservableProperty]
        SurveyQuestionModel lastQuestion;

        [ObservableProperty]
        int nextQCode;

        [ObservableProperty]
        int prevQCode;

        #endregion


        #region UI properties

        [ObservableProperty]
        ImageSource checkMarkImage;

        [ObservableProperty]
        string currentQuestionLbl;

        [ObservableProperty]
        string instructionLbl;

        [ObservableProperty]
        int count;

        [ObservableProperty]
        bool isMissySelectedAnswer;

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
        bool answerIsSelected;

        [ObservableProperty]
        public bool checkmarkIsSelected;

        [ObservableProperty]
        int id;

        [ObservableProperty]
        string sPID;

        [ObservableProperty]
        bool isRefreshing;
       

        [ObservableProperty]
        int textLen = 0;

        [ObservableProperty]
        string qType;

        [ObservableProperty]
        string ruleType;

        [ObservableProperty]
        bool answerHasBeenSelectedForThisQuestion;

        public List<AnswerGroup> UserAnswerGroups { get; private set; } = new List<AnswerGroup>();
        public ObservableCollection<SurveyResponseModel> AnswerCollection { get; set; } = new();

        [ObservableProperty]
        public SurveyModel newSurvey;

        [ObservableProperty]
        public SurveyModel insertedSurvey;

        public ObservableCollection<SurveyModel> SurveyList { get; set; } = new();
        public ObservableCollection<SurveyResponseModel> SurveyResponseList { get; set; } = new();


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
        public async Task CreateSurvey()
        {
            if (IsBusy) return;

            IsBusy = true;
            NewSurvey = new SurveyModel();
            NewSurvey.SurveyDate = DateTime.Now;
            NewSurvey.SurveyStatus = "I";
            NewSurvey.SyncStatus = "I";


            // insert a new record
            await _surveyModelRepository.InsertAsync(NewSurvey);
            InsertedSurvey = NewSurvey;
            await Init();
            IsBusy = false;

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
                if (answer.QCode == CurrentQuestion.QCode)
                {
                    AnswerOptionsForCurrentQuestionCollection.Add(answer);
                }
            }
            // update question in q collection
            var foundQ = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode.Equals(CurrentQuestion.QCode));
            foundQ.IsSelected = true;
            foundQ.PrevQCode = 0;

            IsVisibleSurveyHeader = false;

            // set screen values based on properties in CurrentQuestion
            SetScreenValuesOnOpen();

        }


        #region Navigation

        [RelayCommand]
        public async Task GoNext()
        {
            Console.WriteLine("NextButtonClicked");

            // has an answer been selected
            if (CurrentQuestion.QType == "SingleAnswer" || CurrentQuestion.QType.Equals("MultipleAnswers"))  
            {
                MakeSureAnswerHasBeenSelectedForCurrentQuestion();
            }
            else //(CurrentQuestion.QType.Equals("Text"))
            {
                if (string.IsNullOrEmpty(UserTextAnswer))
                {
                    await _messageService.DisplayAlert("", "Please enter a response", "OK", "Cancel");
                }
                else
                {
                    // set the answer to IsSelected

                    var foundA = AllPossibleAnswerOptionsCollection.FirstOrDefault(x => x.QCode == CurrentQuestion.QCode && x.AText == "");
                    foundA.IsSelected = true;

                    // insert record into
                    SurveyResponseModel responseObj = new SurveyResponseModel();
                    responseObj.SurveyId = Int32.Parse(SPID);
                    responseObj.QCode = CurrentQuestion.QCode;
                    responseObj.QText = CurrentQuestion.QText;
                    responseObj.ACode = foundA.ACode;
                    responseObj.AText = UserTextAnswer;

                    await _surveyResponseModelRepository.InsertAsync(responseObj);

                }
            }

            if(AnswerHasBeenSelectedForThisQuestion == true)
            { 
                var foundCurrQ = CurrentQuestion;
                int ruleType = 0;

                if (CurrentQuestion.QType == "SingleAnswer")   // CurrentQuestion.QType must be SingleAnswer
                {
                   

                    if (UserSelectedAnswer.RuleType == 0 || UserSelectedAnswer.RuleType == -1)  // this is the last q
                    {
                        Debug.WriteLine("NextButtonClicked: this is the last question, go to review");
                        // go to review
                        IsAnswerReview = true;

                        CurrentQuestion.NextQCode = 0;
                        CreateUserResponsesCollection();

                        // set screen values based on properties in CurrentQuestion
                        SetScreenValuesOnOpen();
                    }
                    ruleType = UserSelectedAnswer.RuleType;
                }
                else if (CurrentQuestion.QType == "MultipleAnswers")      // CurrentQuestion.QType must be MultipleAnswers
                {

                    foreach (var answer in UserSelectedAnswers)
                    {
                        if (answer.RuleType == 0 || answer.RuleType == -1)
                        {

                            // go to review

                            Debug.WriteLine("NextButtonClicked: this is the last question, go to review");
                            // go to review

                            CurrentQuestion.NextQCode = 0;
                            CreateUserResponsesCollection();

                            // set screen values based on properties in CurrentQuestion
                            SetScreenValuesOnOpen();
                        }
                        ruleType = answer.RuleType;

                    }
                }
                else
                {
                    Debug.WriteLine("what do do about text");
                }


                var foundNextQ = AllPossibleQuestionsCollection.FirstOrDefault(v => v.QCode.Equals(ruleType));
                if (foundNextQ == null)  // there are no more q
                {
                    Debug.WriteLine("Should CurrentQuestion be null?");
                    //CurrentQuestion = null;
                }
                else  // there is a next q
                {

                    CurrentQuestion.NextQCode = foundNextQ.QCode;
                    // set prev q
                    if (CurrentQuestion.QCode == 1)  // this is the first q
                    {
                        foundCurrQ.PrevQCode = 0;
                    }
                    foundNextQ.PrevQCode = foundCurrQ.QCode;

                    // all prev codes have been updated.

                    // update current question
                    CurrentQuestion = foundNextQ;

                    // update question in q collection
                    foundCurrQ = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode.Equals(CurrentQuestion.QCode));
                    foundCurrQ.IsSelected = true;

                    // get answers for current q
                    AnswerOptionsForCurrentQuestionCollection.Clear();
                    foreach (var i in AllPossibleAnswerOptionsCollection)
                    {
                        if (i.QCode == CurrentQuestion.QCode)
                        {
                            AnswerOptionsForCurrentQuestionCollection.Add(i);
                        }
                    }
                }


                // set screen values based on properties in CurrentQuestion
                SetScreenValuesOnOpen();

            }
            else
            {
                await _messageService.DisplayAlert("", "Please make a selection", "OK", "Cancel");

            }

        }


        [RelayCommand]
        private async void GoBack()
        {
            Console.WriteLine("BackButtonClicked");
            

            // see if it is review page
            if (IsAnswerReview == true)
            {
                var foundQs = AllPossibleQuestionsCollection.Where(x => x.IsSelected.Equals(true));
                CurrentQuestion = foundQs.Last();
            }
            else
            {
                //get new curr q from prev q
                CurrentQuestion = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode.Equals(CurrentQuestion.PrevQCode));
            }

            // get answers for curr q
            AnswerOptionsForCurrentQuestionCollection.Clear();
            foreach (var answer in answerSource)
            {
                if (answer.QCode == CurrentQuestion.QCode)
                {

                    AnswerOptionsForCurrentQuestionCollection.Add(answer);
                }
            }

            if (CurrentQuestion.QType == "SingleAnswer" || CurrentQuestion.QType == "MultipleAnswers")
            {
                MakeSureAnswerHasBeenSelectedForCurrentQuestion();
            }
            else  // CurrentQuestion.QType must be Text
            {
                Debug.WriteLine("Get existing text for back button");
            }

            // set screen values based on properties in CurrentQuestion
            SetScreenValuesOnOpen();

        }

        private void MakeSureAnswerHasBeenSelectedForCurrentQuestion()
        {
            var currentAnswers = AllPossibleAnswerOptionsCollection.Where(x => x.QCode == CurrentQuestion.QCode);
            AnswerHasBeenSelectedForThisQuestion = false;
            foreach (var item in currentAnswers)
            {
                if (item.IsSelected == true)
                {
                    UserSelectedAnswer = item;
                    AnswerHasBeenSelectedForThisQuestion = true;
                }
            }
        }

        private int GetAnswerOptionsForCurrentQuestion()
        {
            AnswerOptionsForCurrentQuestionCollection.Clear();
            var filteredList = AllPossibleAnswerOptionsCollection.Where(t => t.QCode.Equals(CurrentQuestion.QCode));

            AnswerOptionsForCurrentQuestionCollection = new ObservableCollection<SurveyAnswerModel>(filteredList);

            //foreach (var item in itemList) AnswerOptionsForCurrentQuestionList.Add(item);
            return 1;
        }

        #endregion


        #region User Selections

        public ICommand MultipleSelectionCommand => new Command<IList<object>>((obj) =>
        {
            List<SurveyAnswerModel> temp = new List<SurveyAnswerModel>();

            foreach (var item in obj)
            {
                var selectedItems = item as SurveyAnswerModel;
                selectedItems.IsSelected = true;
                temp.Add(selectedItems);
                // Check to see if this is the last question
                if (selectedItems.RuleType != -1)
                {
                    NextQCode = selectedItems.RuleType;
                    AnswerReviewIsNext = false;
                    RightBtnLbl = "Next";
                }
                else
                {
                    NextQCode = -1;
                    AnswerReviewIsNext = true;
                    RightBtnLbl = "Review";
                }

            }
            UserSelectedAnswers.Clear();
            foreach (var item in temp)
            {
                UserSelectedAnswers.Add(item);

            }
        });

        [RelayCommand]
        public async Task AnswerSelected() //Single Answer
        {
            //if (CurrentQuestion.NextQCode == 0)  // the first time the user has selected an answer to this question
            //{
            if (CurrentQuestion.QType == "SingleAnswer")     // CurrentQuestion.QType must be SingleAnswer
            {
                var selectedAnswer = AnswerOptionsForCurrentQuestionCollection.FirstOrDefault(x => x.QCode == UserSelectedAnswer.QCode && x.ACode == UserSelectedAnswer.ACode);
                var otherAnswer = AnswerOptionsForCurrentQuestionCollection.FirstOrDefault(x => x.QCode == UserSelectedAnswer.QCode && x.ACode != UserSelectedAnswer.ACode);

                    selectedAnswer.IsSelected = true;
                    otherAnswer.IsSelected = false;

                if (UserSelectedAnswer.RuleType != -1)
                {
                    NextQCode = UserSelectedAnswer.RuleType;
                    RightBtnLbl = "Next";
                }
                else
                {
                    NextQCode = -1;
                    AnswerReviewIsNext = true;
                    RightBtnLbl = "Review";
                }
            }
        }

        #endregion


        #region UI Screen Values

        public void SetScreenValuesOnOpen()
        {
            SPID = "987654";
            IsVisibleSurveyStartButton = false;
            IsVisibleSurveyHeader = true;

            if (CurrentQuestion != null)
            {
                if (IsAnswerReview == false)
                {
                    IsWorkingRightBtn = true;

                    ScreenNameLbl = CurrentQuestion.QCodeDesc;
                    CurrentQuestionLbl = CurrentQuestion.QText;
                    if (CurrentQuestion.PrevQCode != 0)
                    {
                        LeftBtnLbl = "Back";
                        IsWorkingLeftBtn = true;
                    }
                    else
                    {
                        LeftBtnLbl = "";
                        IsWorkingLeftBtn = false;

                    }
                    RightBtnLbl = "Next";
                    IsVisibleRuleTypeSingle = true;
                    IsVisibleRuleTypeMultiple = false;
                    IsVisibleQTypeText = false;
                    IsVisibleAnswerReview = true;

                    if (CurrentQuestion.QType == "SingleAnswer")     // CurrentQuestion.QType must be SingleAnswer
                    {
                        IsVisibleRuleTypeSingle = true;
                        IsVisibleRuleTypeMultiple = false;
                        IsVisibleQTypeText = false;
                        IsVisibleAnswerReview = false;
                        InstructionLbl = "SINGLE Select an option.";
                    }
                    else if (CurrentQuestion.QType == "MultipleAnswers")     // CurrentQuestion.QType must be MultipleAnswers
                    {
                        IsVisibleRuleTypeSingle = false;
                        IsVisibleRuleTypeMultiple = true;
                        IsVisibleQTypeText = false;
                        IsVisibleAnswerReview = true;

                        InstructionLbl = "MULTIPLE: Select all that apply.";
                    }
                    else // CurrentQuestion.QType must be Text
                    {
                        IsVisibleRuleTypeSingle = false;
                        IsVisibleRuleTypeMultiple = false;
                        IsVisibleAnswerReview = true;
                        IsVisibleQTypeText = true;
                        
                        InstructionLbl = "TEXT: Shat shouild text label be.  Checking character cound.";
                    }
                }
                else
                {
                    ScreenNameLbl = "Review";
                    CurrentQuestionLbl = "Please review your answers here.";
                    LeftBtnLbl = "Back";
                    RightBtnLbl = "";
                    IsWorkingRightBtn = false;
                    InstructionLbl = "";
                    IsVisibleRuleTypeSingle = false;
                    IsVisibleRuleTypeMultiple = false;
                    IsVisibleAnswerReview = true;
                    IsVisibleQTypeText = false;
                    
                }
            }
        }

        #endregion

        
        #region Review Screen

        private void CreateUserResponsesCollection()
        {
            UserAnswerGroups.Clear();
            var dict = AllPossibleAnswerOptionsCollection.Where(x => x.IsSelected.Equals(true)).GroupBy(o => o.QCode)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (KeyValuePair<int, List<SurveyAnswerModel>> item in dict)
            {
                        
                var q = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode == item.Key);
                UserAnswerGroups.Add(new AnswerGroup(item.Key, q.QText, new List<SurveyAnswerModel>(item.Value)));
            }
        }

        public class AnswerGroup : List<SurveyAnswerModel>
        {
            public int QCode { get; set; }
            public string QText { get; set; }

            //public AnswerGroup(string qCode, string qText, List<SurveyResponseModel> userResponses) : base(userResponses)
            public AnswerGroup(int qCode, string qText, List<SurveyAnswerModel> userResponses) : base(userResponses)
            {
                QCode = qCode;
                QText = qText;
            }
        }

        #endregion

    }
}

