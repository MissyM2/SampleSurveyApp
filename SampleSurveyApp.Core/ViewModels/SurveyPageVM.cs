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

        List<SurveyQuestionModel> AllPossibleQuestionsList { get; set; } = new();
        List<SurveyAnswerModel> AllPossibleAnswerOptionsList { get; set; } = new();

        List<SurveyQuestionModel> ActualQuestionsList { get; set; } = new();
        List<SurveyResponseModel> ActualUserSelectedAnswersList { get; set; } = new();

        #endregion

        #region Current Question

        [ObservableProperty]
        int currQuestionIndex;

        [ObservableProperty]
        SurveyQuestionModel currentQuestion;

        [ObservableProperty]
        string currQCode;

        [ObservableProperty]
        SurveyQuestionModel lastQuestion;

        [ObservableProperty]
        string nextQCode;

        [ObservableProperty]
        string prevQCode;

        public ObservableCollection<SurveyAnswerModel> AnswerOptionsForCurrentQuestionList { get; set; } = new();

        #endregion


        #region UI properties
        [ObservableProperty]
        string currentQuestionLbl;

        [ObservableProperty]
        string instructionLbl;

        [ObservableProperty]
        int count;

        [ObservableProperty]
        string screenNameLbl;

        [ObservableProperty]
        string leftBtnLbl;

        [ObservableProperty]
        string rightBtnLbl;

        [ObservableProperty]
        bool isVisibleQuestionTypeText;

        [ObservableProperty]
        bool isVisibleRuleTypeSingle;

        [ObservableProperty]
        bool isVisibleRuleTypeMultiple;

        [ObservableProperty]
        bool isVisibleAnswerReview;

        #endregion

        [ObservableProperty]
        bool reverseDirection;

        [ObservableProperty]
        string selectedListItem;

        [ObservableProperty]
        int id;

        [ObservableProperty]
        string sPID;

        // these two properties (1 object, 1 list of objects are for answers selected by user - either single or multiselecct
        [ObservableProperty]
        public SurveyAnswerModel userSelectedAnswer;
        public ObservableCollection<SurveyAnswerModel> UserSelectedAnswers { get; set; } = new();

        [ObservableProperty]
        public string userTextAnswer;

        [ObservableProperty]
        int textLen = 0;

        [ObservableProperty]
        string questionType;

        [ObservableProperty]
        string ruleType;

        public List<AnswerGroup> UserAnswerGroups { get; private set; } = new List<AnswerGroup>();
        //public ObservableCollection<SurveyResponseModel> AnswerCollection { get; set; } = new();

        [ObservableProperty]
        string qText;

        [ObservableProperty]
        string qCode;

        [ObservableProperty]
        string aText;

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

        }


        #region Navigation
        [RelayCommand]
        public async Task LoadInitialQuestion()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                // get all possible questions and answers
                if (AllPossibleQuestionsList.Any()) AllPossibleQuestionsList.Clear();
                AllPossibleQuestionsList = await _surveyQuestionModelRepository.GetAllAsync();

                if (AllPossibleAnswerOptionsList.Any()) AllPossibleAnswerOptionsList.Clear();
                AllPossibleAnswerOptionsList = await _surveyAnswerModelRepository.GetAllAsync();


                // find the first question in the list and assign it to the current question. Find the last possible question and assign it to LastQuestion.
                // LastQuestion may change..
                if (AllPossibleQuestionsList.Count > 0)
                {

                    // get the current Question
                    CurrentQuestion = AllPossibleQuestionsList[0];

                    ActualQuestionsList.Add(new SurveyQuestionModel
                    {
                        QText = CurrentQuestion.QText,
                        QCode = CurrentQuestion.QCode,
                        RuleType = CurrentQuestion.RuleType,
                        QType = CurrentQuestion.QType
                    });
                    CurrQCode = CurrentQuestion.QCode;

                    // get answers for the current question
                    var rtn = GetAnswerOptionsForCurrentQuestion();

                    // set screen values based on properties in CurrentQuestion
                    var rtn1 = SetScreenValues();
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get list values: {ex.Message}");
                await _messageService.DisplayAlert("Error", "Failed to retrieve survey list values", "OK", "Cancel");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public async Task NextButtonClicked()
        {
            Console.WriteLine("NextButtonClicked");


            // find index of currentquestion and update with proper nextQCode

           
            // Save selected answer(s)
            await SaveUserSelectedAnswers();

            // find the index of the current q
            int findIndex = ActualQuestionsList.FindIndex(v => v.QCode.Equals(CurrQCode));

            // update currIndex with index of currentquestion
            CurrQuestionIndex = findIndex;

            // update current q with nextq and update nextq property
            NextQCode = UserSelectedAnswer.RuleType;
            ActualQuestionsList[findIndex].nextQCode = NextQCode;

            // after confirming save of userSelectedAnswer(s), Save currentQ to ActualQuestionsList

            // determine what screen comes next

            if (CurrQuestionIndex == 0)  // this is the first q
            {
                if (UserSelectedAnswer.ACode == "DONE") // this is the first q and the last q.  there is no prevQ and no nextQ
                {
                    // go to review
                }
                else // this is the first question.  there is no prevQ but there is a nextQ
                {
                    PrevQCode = "";
                    ActualQuestionsList[CurrQuestionIndex].prevQCode = PrevQCode;

                    // check to see if CurrQCode already exists

                    bool alreadyContainsQ = ActualQuestionsList.Any(item => item.QCode == CurrQCode);

                    
                        // get the next question and assign to currentQ
                        CurrentQuestion = AssignCurrentQuestion(NextQCode);

                        // assign currQCode and prevQCode.  You cannot assign next because it comes from next user's answer
                        PrevQCode = CurrQCode;
                        CurrQCode = CurrentQuestion.QCode;

                   
                        ActualQuestionsList.Add(new SurveyQuestionModel
                        {
                            QText = CurrentQuestion.QText,
                            QCode = CurrentQuestion.QCode,
                            RuleType = CurrentQuestion.RuleType,
                            QType = CurrentQuestion.QType
                        });
                    CurrQCode = CurrentQuestion.QCode;



                    // get answers for the current question
                    var rtn = GetAnswerOptionsForCurrentQuestion();

                    // set screen values based on properties in CurrentQuestion
                    var rtn1 = SetScreenValues();
                }
            }
            else // this is not the first q
            {
                if (UserSelectedAnswer.RuleType == "DONE")  // this is the last question.  there is a prevQ but no next q.  go to reveiw
                {
                    ActualQuestionsList[CurrQuestionIndex].prevQCode = PrevQCode;

                    // only add question to list if it does not exist
                    bool alreadyContainsQ = ActualQuestionsList.Any(item => item.QCode == CurrQCode);

                   
                        // get the next question and assign to currentQ
                        //CurrentQuestion = AssignCurrentQuestion(NextQCode);

                        // assign currQCode and prevQCode.  You cannot assign next because it comes from next user's answer
                       // PrevQCode = CurrQCode;
                        ///CurrQCode = CurrentQuestion.QCode;
                    if (alreadyContainsQ == false)
                    {
                        ActualQuestionsList.Add(new SurveyQuestionModel
                        {
                            QText = CurrentQuestion.QText,
                            QCode = CurrentQuestion.QCode,
                            RuleType = CurrentQuestion.RuleType,
                            prevQCode = CurrQCode,
                            nextQCode = "",
                            QType = CurrentQuestion.QType
                        });

                        CurrQCode = CurrentQuestion.QCode;
                    }
                    CurrQCode = CurrentQuestion.QCode;
                    NextQCode = "";

                    CurrentQuestion = null;


                    Debug.WriteLine("Go to review");
                    // go to review
                    //LoadAnswerCollection();
                    CreateUserResponsesCollection();

                    var rtn3 = SetScreenValues();
                }
                else  // this is not the first q and not the last q.  there is a prevQ and a nextQ
                {
                    ActualQuestionsList[CurrQuestionIndex].prevQCode = PrevQCode;

                    // get the next question and assign to currentQ
                    CurrentQuestion = AssignCurrentQuestion(NextQCode);

                    PrevQCode = CurrQCode;
                    CurrQCode = CurrentQuestion.QCode;

                    bool alreadyContainsQ = ActualQuestionsList.Any(item => item.QCode == CurrQCode);

                    if (alreadyContainsQ == false)
                    {
                        // get the next question and assign to currentQ
                        CurrentQuestion = AssignCurrentQuestion(NextQCode);

                        // assign currQCode and prevQCode.  You cannot assign next because it comes from next user's answer
                        //PrevQCode = CurrQCode;
                        CurrQCode = CurrentQuestion.QCode;

                        ActualQuestionsList.Add(new SurveyQuestionModel
                        {
                            QText = CurrentQuestion.QText,
                            QCode = CurrentQuestion.QCode,
                            RuleType = CurrentQuestion.RuleType,
                            QType = CurrentQuestion.QType
                        });
                        CurrQCode = CurrentQuestion.QCode;

                    }
                    else
                    {

                        // get current q from actual questions list if the question already exists (i.e. next button after prev button)
                        CurrentQuestion = ActualQuestionsList.Find(x => x.QCode == NextQCode);
                    }

                    //return AllPossibleQuestionsList.Find(x => x.QCode.Equals(ruleType));

                    // get answers for the current question
                    var rtn = GetAnswerOptionsForCurrentQuestion();

                    // set screen values based on properties in CurrentQuestion
                    var rtn1 = SetScreenValues();
                }

            }

            
        }

        [RelayCommand]
        private async void BackButtonClicked()
        {
            Console.WriteLine("BackButtonClicked");

            CurrentQuestion = ActualQuestionsList[CurrQuestionIndex];
            
            var rtn = GetAnswerOptionsForCurrentQuestion();
            var rtn1 = SetScreenValues();

            CurrQuestionIndex = CurrQuestionIndex - 1;

        }


        #endregion


        async Task<int> SaveUserSelectedAnswers()
        {
            if (CurrentQuestion.RuleType.Equals("Single"))
            {
                if (UserSelectedAnswer == null)
                {
                    await _messageService.DisplayAlert("", "Please make a selection", "OK", "Cancel");
                }
                else
                {

                    int findIndex = ActualUserSelectedAnswersList.FindIndex(v => v.QCode.Equals(CurrentQuestion.QCode));
                    if (findIndex < 0)
                        await SaveSelected();
                }
            }
            else if (CurrentQuestion.RuleType.Equals("Multiple"))
            {
                if (UserSelectedAnswers.Count <= 0)
                {
                    await _messageService.DisplayAlert("", "Please make a selection", "OK", "Cancel");
                }
                else
                {
                    var findAllList = ActualUserSelectedAnswersList.FindAll(v => v.QCode.Equals(CurrentQuestion.QCode));
                    if (findAllList.Count == 0)
                    {
                        await SaveSelected();
                    }
                }

            }
            else if (CurrentQuestion.QType.Equals("Text"))
            {

                if (UserTextAnswer == null)
                {
                    await _messageService.DisplayAlert("", "Please add your response", "OK", "Cancel");
                }
                else
                {
                    await SaveSelected();
                }
            }
            return 1;
        }

        // Get new current question

        //var GetCurrentQuestionReturn = await GetCurrentQuestion();

        //if (GetCurrentQuestionReturn == 1)
        //{
        //    // set screen values based on properties in CurrentQuestion
        //    var setScreenValuesReturn = await SetScreenValues();
        //    if (setScreenValuesReturn == 1)
        //    {
        //        // get answers for currentQuestion
        //        var getAnswersForCurrentQuestionReturn = await GetAnswerOptionsForCurrentQuestion();
        //    }

        //}



        // Assign next current Question

        //    if (CurrentQuestion != null)
        //{
        //    // Save user question to ActualQuestionsList
        //    ActualQuestionsList.Add(CurrentQuestion);

        // Save user answer to ActualUserSelectedAnswersList


        //if (IsFinalQuestion == true)
        //    {

        //        Debug.WriteLine("GO TO REVIEW!");
        //        var setScreenValuesReturn = await SetScreenValues();
        //        if (setScreenValuesReturn == 1)
        //        {
        //            // get answers for currentQuestion
        //            CreateUserResponsesCollection();
        //        }

        //    }
        //    else
        //    {



        //     }
        //    }
        //}



        #region Questions

        private SurveyQuestionModel AssignCurrentQuestion(string ruleType)
        {
            return AllPossibleQuestionsList.Find(x => x.QCode.Equals(ruleType));
        }

        #endregion

        #region UI

        public int SetScreenValues()
        {
            SPID = "987654";

            if (CurrentQuestion != null)  
            {
                ScreenNameLbl = CurrentQuestion.QCode;
                CurrentQuestionLbl = CurrentQuestion.QText;

                if (CurrQuestionIndex == 0) // this is the first q
                {
                    LeftBtnLbl = "";
                }
                else
                {
                    LeftBtnLbl = "Back";
                }

                if (NextQCode == "")  // this is the last question
                {
                    RightBtnLbl = "Review";
                }
                else
                {
                    RightBtnLbl = "Next";
                }

                if (CurrentQuestion.QType == "List")
                {
                    if (CurrentQuestion.RuleType == "Single")
                    {
                        IsVisibleRuleTypeSingle = true;
                        IsVisibleRuleTypeMultiple = false;
                        IsVisibleAnswerReview = false;
                        IsVisibleQuestionTypeText = false;
                        InstructionLbl = "SINGLE Select an option.";
                    }
                    else // CurrentQuestion.RuleType must be multiple
                    {
                        IsVisibleRuleTypeSingle = false;
                        IsVisibleRuleTypeMultiple = true;
                        IsVisibleAnswerReview = false;
                        IsVisibleQuestionTypeText = false;
                        InstructionLbl = "MULTIPLE: Select all that apply.";
                    }

                }
                else // CurrentQuestion.QuestionType can only be Text
                {
                    IsVisibleRuleTypeSingle = false;
                    IsVisibleRuleTypeMultiple = false;
                    IsVisibleQuestionTypeText = true;
                    IsVisibleAnswerReview = false;
                    InstructionLbl = "TEXT: Shat shouild text label be.  Checking character cound.";
                }
            }
            else // must be review page
            {
                ScreenNameLbl = "Review Page";
                CurrentQuestionLbl = "Please review your answers here.";
                LeftBtnLbl = "Back";
                IsVisibleRuleTypeSingle = false;
                IsVisibleRuleTypeMultiple = false;
                IsVisibleQuestionTypeText = false;
                IsVisibleAnswerReview = true;
            }

            return 1;
        }

        #endregion

        #region Answers

        private int GetAnswerOptionsForCurrentQuestion()
        {
            AnswerOptionsForCurrentQuestionList.Clear();
            var itemList = AllPossibleAnswerOptionsList.FindAll(t => t.ValueType.Equals(CurrentQuestion.QCode));

            foreach (var item in itemList) AnswerOptionsForCurrentQuestionList.Add(item);
            return 1;
        }

        #endregion

       
        [RelayCommand]
        public async Task SingleAnswerSelected()
        {
            Debug.WriteLine("Single option selected");
            SurveyAnswerModel tempAnswer = new SurveyAnswerModel();
            tempAnswer = UserSelectedAnswer;

            // check to see if there are any questions after this answer is added
            if (tempAnswer.RuleType.ToLower().Equals("done"))
            {
                NextQCode = null;
                //PrevQCode = ?;
            }
            else
            {
                NextQCode = tempAnswer.ACode;
                //PrevQCode = ?;
            }
            UserSelectedAnswers.Add(tempAnswer);
        }

            [RelayCommand]
        public async Task ResponseChanged(object responseParams)
        {
            UserSelectedAnswers.Clear();
            
            List<SurveyAnswerModel> myListItems = ((IEnumerable)responseParams).Cast<SurveyAnswerModel>().ToList();

            // check to see if there are any questions after this answer is added
            var tempResponse = myListItems.FirstOrDefault(x => x.RuleType.ToLower().Equals("done"));
            if(tempResponse != null)
            {
                NextQCode = null;
                //PrevQCode = ?;
            }
            else
            {
                NextQCode = tempResponse.ACode;
                //PrevQCode = ?;
            }



            UserSelectedAnswers = new ObservableCollection<SurveyAnswerModel>(myListItems);


            Debug.WriteLine("Count of selected responses in parameter = " + UserSelectedAnswers.Count.ToString());

            
            //else if (CurrentQuestion.RuleType.Equals("Text"))
            //{
            //    await _messageService.DisplayAlert("Text Question", "Add text question here", "OK", null);
            //}

        }

        public async Task<int> SaveSelected()
        {

            if (CurrentQuestion.RuleType.Equals("Single"))
            {
                SurveyResponseModel responseObj = new SurveyResponseModel();
                responseObj.QCode = CurrQCode;
                responseObj.QText = CurrentQuestion.QText;
                responseObj.ACode = UserSelectedAnswer.ACode;
                responseObj.Id = Id;
                responseObj.AText = UserSelectedAnswer.AText;

                ActualUserSelectedAnswersList.Add(responseObj);
            }
            if (CurrentQuestion.RuleType.Equals("Multiple"))
            {
                foreach (var item in UserSelectedAnswers)
                {
                    SurveyResponseModel responseObj = new SurveyResponseModel();
                    responseObj.QCode = CurrQCode;
                    responseObj.QCode = CurrQCode;
                    responseObj.QText = CurrentQuestion.QText;
                    responseObj.Id = Id;
                    responseObj.ACode = item.ACode;
                    responseObj.AText = item.AText;

                    ActualUserSelectedAnswersList.Add(responseObj);
                }
            }

            if (CurrentQuestion.QType.Equals("Text"))
            {
                foreach (var item in UserSelectedAnswers)
                {
                    SurveyResponseModel responseObj = new SurveyResponseModel();
                    responseObj.QCode = CurrQCode;
                    responseObj.QText = CurrentQuestion.QText;
                    responseObj.Id = Id;
                    responseObj.ACode = "Text";
                    responseObj.AText = UserTextAnswer;

                    ActualUserSelectedAnswersList.Add(responseObj);
                }
            }

            return 1;

            //foreach(var item in responseList)
            //{
            //    await _databaseHelper.InsertData(item);
            //}
        }

        //[RelayCommand]
        //public void LoadAnswerCollection()
        //{
        //    foreach (var item in ActualUserSelectedAnswersList)
        //    {
        //        AnswerCollection.Add(item);
        //    }

        //    CreateUserResponsesCollection();
        //}

        private void CreateUserResponsesCollection()
        {
            UserAnswerGroups.Clear();

            var dict = ActualUserSelectedAnswersList.GroupBy(o => o.QCode)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (KeyValuePair<string, List<SurveyResponseModel>> item in dict)
            {
                var q = ActualQuestionsList.Find(x => x.QCode == item.Key);
                UserAnswerGroups.Add(new AnswerGroup(item.Key, q.QText, new List<SurveyResponseModel>(item.Value)));
            }
        }


        //[RelayCommand]
        //private void SelectOrientation()
        //{
        //    var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
        //    if (mainDisplayInfo.Width != width || mainDisplayInfo.Height != height)
        //    {
        //        width = mainDisplayInfo.Width;
        //        height = mainDisplayInfo.Height;
        //    }

        //    if (width > height)
        //    {
        //        IsPortraitOrientation = false;
        //        IsLandscapeOrientation = true;
        //    }
        //    else
        //    {
        //        IsPortraitOrientation = true;
        //        IsLandscapeOrientation = false;
        //    }
        //}

        public class AnswerGroup : List<SurveyResponseModel>
        {
            public string QCode { get; set; }
            public string QText { get; set; }

            //public AnswerGroup(string qCode, string qText, List<SurveyResponseModel> userResponses) : base(userResponses)
            public AnswerGroup(string qCode, string qText, List<SurveyResponseModel> userResponses) : base(userResponses)
            {
                QCode = qCode;
                QText = qText;
            }


        }
       
    }
}

