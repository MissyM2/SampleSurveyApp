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
using System.Collections.Generic;

namespace SampleSurveyApp.Core.ViewModels
{
    [QueryProperty("Id", "Id")]
    public partial class SurveyPageVM : BaseVM
    {

        #region Dependency Injection

        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        private readonly IUserPreferences _userPreferences;

        private readonly IAsyncRepository<SurveyValuesModel> _surveyValuesModelRepository;
        private readonly IAsyncRepository<SurveyModel> _surveyModelRepository;
        private readonly IAsyncRepository<SurveyResponseModel> _surveyResponseModelRepository;

        #endregion

        #region Q and A lists

        List<SurveyValuesModel> AllPossibleQuestionsList { get; set; } = new();
        List<SurveyValuesModel> AllPossibleAnswerOptionsList { get; set; } = new();

        List<SurveyValuesModel> ActualQuestionsList { get; set; } = new();
        List<SurveyResponseModel> ActualUserSelectedAnswersList { get; set; } = new();

        #endregion

        #region Current Question

        [ObservableProperty]
        int currQuestionIndex;

        [ObservableProperty]
        SurveyValuesModel currentQuestion;

        [ObservableProperty]
        SurveyValuesModel lastQuestion;

        [ObservableProperty]
        string nextQuestionRuleType;

        [ObservableProperty]
        string prevQuestionRuleType;

        public ObservableCollection<SurveyValuesModel> AnswerOptionsForCurrentQuestionList { get; set; } = new();

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
        public SurveyValuesModel userSelectedAnswer;

        public ObservableCollection<SurveyValuesModel> UserSelectedAnswers { get; set; } = new();

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
        string questionText;

        [ObservableProperty]
        string answerText;

        public SurveyPageVM(
            INavigationService navigationService,
            IMessageService messageService,
            IUserPreferences userPreferences,
            IAsyncRepository<SurveyValuesModel> surveyValuesModelRepository,
            IAsyncRepository<SurveyModel> surveyModelRepository,
            IAsyncRepository<SurveyResponseModel> surveyResponseModelRepository)
        {
            _navigationService = navigationService;
            _messageService = messageService;
            _userPreferences = userPreferences;
            _surveyValuesModelRepository = surveyValuesModelRepository;
            _surveyModelRepository = surveyModelRepository;
            _surveyResponseModelRepository = surveyResponseModelRepository;

        }



        [RelayCommand]
        public async Task LoadInitialQuestion()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                // get all possible questions and answers
                if (AllPossibleQuestionsList.Any()) AllPossibleQuestionsList.Clear();
                AllPossibleQuestionsList = await _surveyValuesModelRepository.GetWhereAsync(q => q.SurveyValueType.Equals("Questions"));

                if (AllPossibleAnswerOptionsList.Any()) AllPossibleAnswerOptionsList.Clear();
                AllPossibleAnswerOptionsList = await _surveyValuesModelRepository.GetWhereAsync(q => q.SurveyValueType.Equals("Answers"));


                // find the first question in the list and assign it to the current question. Find the last possible question and assign it to LastQuestion.
                // LastQuestion may change..
                if (AllPossibleQuestionsList.Count > 0)
                {

                    // get the current Question
                    CurrentQuestion = AllPossibleQuestionsList[0];

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
            ActualQuestionsList.Add(CurrentQuestion);

            // determine what screen comes next

            if (PrevQuestionRuleType == null)  // this is the first q
            {
                if (UserSelectedAnswer.ValueCode == "DONE") // this is the first q and the last q.  there is no prevQ and no nextQ
                {
                    // go to review
                }
                else // this is the first question.  there is no prevQ but there is a nextQ
                {
                    PrevQuestionRuleType = NextQuestionRuleType;
                    NextQuestionRuleType = UserSelectedAnswer.RuleType;

                    // get the next question and assign to currentQ
                    CurrentQuestion = AssignCurrentQuestion(NextQuestionRuleType);

                    // get answers for the current question
                    var rtn = GetAnswerOptionsForCurrentQuestion();

                    // set screen values based on properties in CurrentQuestion
                    var rtn1 = SetScreenValues();
                }
            }
            else // this is not the first q
            {
                if (UserSelectedAnswer.ValueCode == "DONE")  // this is the last question.  there is a prevQ but no next q.  go to reveiw
                {
                    PrevQuestionRuleType = NextQuestionRuleType;
                    NextQuestionRuleType = null;
                    Debug.WriteLine("Go to review");
                    // go to review
                }
                else  // this is not the first q and not the last q.  there is a prevQ and a nextQ
                {
                    PrevQuestionRuleType = NextQuestionRuleType;
                    NextQuestionRuleType = UserSelectedAnswer.RuleType;

                    // get the next question and assign to current
                    CurrentQuestion = AssignCurrentQuestion(NextQuestionRuleType);

                    // get answers for the current question
                    var rtn = GetAnswerOptionsForCurrentQuestion();

                    // set screen values based on properties in CurrentQuestion
                    var rtn1 = SetScreenValues();
                }

            }
        }

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

                    int findIndex = ActualUserSelectedAnswersList.FindIndex(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
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
                    var findAllList = ActualUserSelectedAnswersList.FindAll(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
                    if (findAllList.Count == 0)
                    {
                        await SaveSelected();
                    }
                }

            }
            else if (CurrentQuestion.QuestionType.Equals("Text"))
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

        private SurveyValuesModel AssignCurrentQuestion(string ruleType)
        {
            return AllPossibleQuestionsList.Find(x => x.ValueCode.Equals(ruleType));
        }

        #endregion

        #region UI

        public int SetScreenValues()
        {
            SPID = "987654";

            if (CurrentQuestion != null)
            {
                ScreenNameLbl = CurrentQuestion.ValueCode;
                CurrentQuestionLbl = CurrentQuestion.ValueText;

                if (CurrQuestionIndex==0)
                {
                    LeftBtnLbl = "";
                }
                else
                {
                    LeftBtnLbl = "Back";
                }

                // not right.. first question will not have a nextqvaluecode
                if (NextQuestionRuleType == null)
                {
                    RightBtnLbl = "Review";
                }
                else
                {
                    RightBtnLbl = "Next";
                }

                if (CurrentQuestion.QuestionType == "List")
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
            var itemList = AllPossibleAnswerOptionsList.FindAll(t => t.ValueType.Equals(CurrentQuestion.ValueCode));

            foreach (var item in itemList) AnswerOptionsForCurrentQuestionList.Add(item);
            return 1;
        }

        #endregion

        [RelayCommand]
        private async void BackButtonClicked()
        {
            Console.WriteLine("BackButtonClicked");
            if (CurrentQuestion != null)
            {
                //// Save user question to ActualQuestionsList
                //ActualQuestionsList.Add(CurrentQuestion);

                //// find the index of the question in ActualQuestionsList that matches the currentquestion valuecode and display
                //int currQuestionIndex = ActualQuestionsList.FindIndex(x => x.ValueCode == CurrentQuestion.ValueCode);
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
                //        //    var tempSelectedAnswer = ActualUserSelectedAnswersList.Find(t => t.QuestionCode.Equals(CurrentQuestion.ValueCode));

                //        //    // find the answer in the current question list that has been selected
                //        //    var userSelectedOption = AnswerOptionsForCurrentQuestionList.FirstOrDefault(t => t.ValueCode == tempSelectedAnswer.AnswerCode);

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


      

        [RelayCommand]
        public async Task SingleAnswerSelected()
        {
            Debug.WriteLine("Single option selected");
            SurveyValuesModel tempAnswer = new SurveyValuesModel();
            tempAnswer = UserSelectedAnswer;

            // check to see if there are any questions after this answer is added
            if (tempAnswer.RuleType.ToLower().Equals("done"))
            {
                NextQuestionRuleType = null;
                //PrevQuestionRuleType = ?;
            }
            else
            {
                NextQuestionRuleType = tempAnswer.ValueCode;
                //PrevQuestionRuleType = ?;
            }
            UserSelectedAnswers.Add(tempAnswer);
        }

            [RelayCommand]
        public async Task ResponseChanged(object responseParams)
        {
            UserSelectedAnswers.Clear();
            
            List<SurveyValuesModel> myListItems = ((IEnumerable)responseParams).Cast<SurveyValuesModel>().ToList();

            // check to see if there are any questions after this answer is added
            var tempResponse = myListItems.FirstOrDefault(x => x.RuleType.ToLower().Equals("done"));
            if(tempResponse != null)
            {
                NextQuestionRuleType = null;
                //PrevQuestionRuleType = ?;
            }
            else
            {
                NextQuestionRuleType = tempResponse.ValueCode;
                //PrevQuestionRuleType = ?;
            }



            UserSelectedAnswers = new ObservableCollection<SurveyValuesModel>(myListItems);


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
                responseObj.QuestionCode = CurrentQuestion.ValueCode;
                responseObj.QuestionText = CurrentQuestion.ValueText;
                responseObj.AnswerCode = UserSelectedAnswer.ValueCode;
                responseObj.Id = Id;
                responseObj.AnswerText = UserSelectedAnswer.ValueText;

                ActualUserSelectedAnswersList.Add(responseObj);
            }
            if (CurrentQuestion.RuleType.Equals("Multiple"))
            {
                foreach (var item in UserSelectedAnswers)
                {
                    SurveyResponseModel responseObj = new SurveyResponseModel();
                    responseObj.QuestionCode = CurrentQuestion.ValueCode;
                    responseObj.QuestionText = CurrentQuestion.ValueText;
                    responseObj.Id = Id;
                    responseObj.AnswerCode = item.ValueCode;
                    responseObj.AnswerText = item.ValueText;

                    ActualUserSelectedAnswersList.Add(responseObj);
                }
            }

            if (CurrentQuestion.QuestionType.Equals("Text"))
            {
                foreach (var item in UserSelectedAnswers)
                {
                    SurveyResponseModel responseObj = new SurveyResponseModel();
                    responseObj.QuestionCode = CurrentQuestion.ValueCode;
                    responseObj.QuestionText = CurrentQuestion.ValueText;
                    responseObj.Id = Id;
                    responseObj.AnswerCode = "Text";
                    responseObj.AnswerText = UserTextAnswer;

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

            var dict = ActualUserSelectedAnswersList.GroupBy(o => o.QuestionText)
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
            public string QuestionText { get; private set; }

            public ResponseGroup(string questionText, List<SurveyResponseModel> userResponses) : base(userResponses)
            {
                QuestionText = questionText;
            }
        }
    }
}

