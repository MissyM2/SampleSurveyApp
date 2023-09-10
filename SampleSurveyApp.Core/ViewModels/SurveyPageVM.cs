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


        // this  list is for all responses available to user
        public List<SurveyResponseModel> responseList { get; set; } = new();

        [ObservableProperty]
        string questionType;

        [ObservableProperty]
        string ruleType;

        public List<ResponseGroup> UserResponseGroups { get; private set; } = new List<ResponseGroup>();
        public ObservableCollection<SurveyResponseModel> ResponseCollection { get; set; } = new();
        public List<SurveyResponseModel> UserResponseList { get; set; } = new();

        [ObservableProperty]
        string qText;

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
                    CurrQCode = CurrentQuestion.QCode;
                    PrevQCode = "";

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

            // Save selected answer(s)
            await SaveUserSelectedAnswers();

            


            // after confirming save of userSelectedAnswer(s), Save currentQ to ActualQuestionsList

            // determine what screen comes next

            if (PrevQCode == "")  // this is the first q
            {
                if (UserSelectedAnswer.ACode == "DONE") // this is the first q and the last q.  there is no prevQ and no nextQ
                {
                    // go to review
                }
                else // this is the first question.  there is no prevQ but there is a nextQ
                {
                    // save current q,using existing Rules

                    ActualQuestionsList.Add(new SurveyQuestionModel
                    {
                        QText = CurrentQuestion.QText,
                        QCode = CurrentQuestion.QCode,
                        prevQCode = "",
                        nextQCode = UserSelectedAnswer.RuleType,
                        RuleType = CurrentQuestion.RuleType,
                        QType = CurrentQuestion.QType
                    });
                    CurrQCode = CurrentQuestion.QCode;
                    NextQCode = UserSelectedAnswer.RuleType;
                    PrevQCode = "";

                    // get the next question and assign to currentQ
                    CurrentQuestion = AssignCurrentQuestion(NextQCode);

                    // assign currQCode and prevQCode.  You cannot assign next because it comes from next user's answer
                    PrevQCode = CurrQCode;
                    CurrQCode = CurrentQuestion.QCode;
                    
                    

                    // get answers for the current question
                    var rtn = GetAnswerOptionsForCurrentQuestion();

                    // set screen values based on properties in CurrentQuestion
                    var rtn1 = SetScreenValues();
                }
            }
            else // this is not the first q
            {
                if (UserSelectedAnswer.ACode == "DONE")  // this is the last question.  there is a prevQ but no next q.  go to reveiw
                {

                    // save current q,using existing Rules
                    ActualQuestionsList.Add(new SurveyQuestionModel
                    {
                        QText = CurrentQuestion.QText,
                        QCode = CurrentQuestion.QCode,
                        prevQCode = CurrentQuestion.prevQCode,
                        nextQCode = "",
                        RuleType = CurrentQuestion.RuleType,
                        QType = CurrentQuestion.QType
                    });

                    // assign currQCode and prevQCode.  You cannot assign next because there are no more questions.
                    CurrQCode = CurrentQuestion.QCode;
                    NextQCode = "";
                    PrevQCode = CurrentQuestion.prevQCode;


                    Debug.WriteLine("Go to review");
                    // go to review
                }
                else  // this is not the first q and not the last q.  there is a prevQ and a nextQ
                {

                    // save current q,using existing Rules
                    ActualQuestionsList.Add(new SurveyQuestionModel
                    {
                        QText = CurrentQuestion.QText,
                        QCode = CurrentQuestion.QCode,
                        prevQCode = CurrentQuestion.prevQCode,
                        nextQCode = UserSelectedAnswer.RuleType,
                        RuleType = CurrentQuestion.RuleType,
                        QType = CurrentQuestion.QType
                    });
                    // set rule types
                    currQCode = CurrentQuestion.QCode;
                    PrevQCode = CurrQCode;
                    NextQCode = UserSelectedAnswer.RuleType;

                    // get the next question and assign to current
                    CurrentQuestion = AssignCurrentQuestion(UserSelectedAnswer.RuleType);
                    CurrQCode = UserSelectedAnswer.RuleType;

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

            // Save selected answer(s)
            await SaveUserSelectedAnswers();

            // after confirming save of userSelectedAnswer(s), Save currentQ to ActualQuestionsList
            ActualQuestionsList.Add(CurrentQuestion);

            // determine what screen comes next

            if (PrevQCode == null)  // this is the first q
            {
                if (UserSelectedAnswer.ACode == "DONE") // this is the first q and the last q.  there is no prevQ and no nextQ
                {
                    // go to review
                }
                else // this is the first question.  there is no prevQ but there is a nextQ
                {
                    PrevQCode = "";
                    NextQCode = UserSelectedAnswer.RuleType;

                    // get the next question and assign to currentQ
                    CurrentQuestion = AssignCurrentQuestion(PrevQCode);

                    // get answers for the current question
                    var rtn = GetAnswerOptionsForCurrentQuestion();

                    //FOR BACK BUTTON NEED THIS
                    // get the answers selected for that question
                    var tempAnswers = ActualUserSelectedAnswersList.FindAll(x => x.QCode.Equals(CurrentQuestion.ValueType));

                    if (tempAnswers.Count > 0)  // if so go to prev..this assumes that they have decided not to change the answers
                    {

                    }
                    else
                    {

                    }

                    // set screen values based on properties in CurrentQuestion
                    var rtn1 = SetScreenValues();
                }
            }
            else // this is not the first q
            {
                if (UserSelectedAnswer.ACode == "DONE")  // this is the last question.  there is a prevQ but no next q.  go to reveiw
                {
                    PrevQCode = NextQCode;
                    NextQCode = null;
                    Debug.WriteLine("Go to review");
                    // go to review
                }
                else  // this is not the first q and not the last q.  there is a prevQ and a nextQ
                {
                    CurrentQuestion = AssignCurrentQuestion(PrevQCode);
                    PrevQCode = NextQCode;
                    NextQCode = UserSelectedAnswer.RuleType;

                    // get the next question and assign to current
                    //CurrentQuestion = AssignCurrentQuestion(PrevQCode);

                    // get answers for the current question
                    var rtn = GetAnswerOptionsForCurrentQuestion();

                    //FOR BACK BUTTON NEED THIS
                    // get the answers selected for that question
                    var tempAnswers = ActualUserSelectedAnswersList.FindAll(x => x.QCode.Equals(CurrentQuestion.ValueType));

                    if (tempAnswers.Count > 0)  // if so go to prev..this assumes that they have decided not to change the answers
                    {

                    }
                    else
                    {

                    }

                    // set screen values based on properties in CurrentQuestion
                    var rtn1 = SetScreenValues();
                }


                //// Save user question to ActualQuestionsList
                //ActualQuestionsList.Add(CurrentQuestion);

                //// find the index of the question in ActualQuestionsList that matches the currentquestion valuecode and display
                //int currQuestionIndex = ActualQuestionsList.FindIndex(x => x.QCode == CurrentQuestion.QCode);
                //if (currQuestionIndex == 0)
                //{
                //    return;
                //}
                //else
                //{
                //    CurrentQuestion = ActualQuestionsList[currQuestionIndex - 1];

                //    if (CurrentQuestion.RuleType.Equals("Single"))
                //    {

                //    }



                //    var setScreenValuesReturn = await SetScreenValues();
                //    if (setScreenValuesReturn == 1)
                //    {
                //        // get answers for currentQuestion
                //        var getAnswersForCurrentQuestionReturn = GetAnswerOptionsForCurrentQuestion();

                //        //if (CurrentQuestion.RuleType.Equals("Single"))
                //        //{
                //        //    // see which answer(s) that the user selected in Response Format
                //        //    var tempSelectedAnswer = ActualUserSelectedAnswersList.Find(t => t.QCode.Equals(CurrentQuestion.QCode));

                //        //    // find the answer in the current question list that has been selected
                //        //    var userSelectedOption = AnswerOptionsForCurrentQuestionList.FirstOrDefault(t => t.ACode == tempSelectedAnswer.ACode);

                //        //    // once you get the answer(s) the at the user selected, place a checkmark next to them.
                //        //    SelectedResponse = userSelectedOption;
                //        //}
                //        if (CurrentQuestion.RuleType.Equals("Multiple"))
                //        {
                //            Debug.WriteLine("Multiple");

                //        }
                //        else if (CurrentQuestion.QuestionType.Equals("Text"))
                //        {
                //            Debug.WriteLine("Multiple");

                //        }
                //    }
                //}
            }
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

                if (CurrQuestionIndex==0)
                {
                    LeftBtnLbl = "";
                }
                else
                {
                    LeftBtnLbl = "Back";
                }

                // not right.. first question will not have a nextqvaluecode
                if (NextQCode == null)
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
                ScreenNameLbl = "Review Page";

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

        private void CreateUserResponsesCollection()
        {
            UserResponseGroups.Clear();

            var dict = ActualUserSelectedAnswersList.GroupBy(o => o.QText)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (KeyValuePair<string, List<SurveyResponseModel>> item in dict)
            {
                UserResponseGroups.Add(new ResponseGroup(item.Key, new List<SurveyResponseModel>(item.Value)));
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

        public class ResponseGroup : List<SurveyResponseModel>
        {
            public string QText { get; private set; }

            public ResponseGroup(string QText, List<SurveyResponseModel> userResponses) : base(userResponses)
            {
                QText = QText;
            }
        }
       
    }
}

