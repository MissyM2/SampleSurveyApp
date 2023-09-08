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
        List<SurveyValuesModel> AllPossibleAnswersList { get; set; } = new();

        List<SurveyValuesModel> ActualQuestionsList { get; set; } = new();
        List<SurveyResponseModel> ActualUserSelectedAnswersList { get; set; } = new();

        #endregion

        #region Current Question

        [ObservableProperty]
        int currentIndexNum;

        [ObservableProperty]
        SurveyValuesModel currentQuestion;

        [ObservableProperty]
        SurveyValuesModel lastQuestion;

        [ObservableProperty]
        bool isFinalQuestion;

        public ObservableCollection<SurveyValuesModel> AnswersForCurrentQuestionList { get; set; } = new();

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
        bool isVisibleQuestionTypeList;

        [ObservableProperty]
        bool isVisibleAnswerReview;

        [ObservableProperty]
        Microsoft.Maui.Controls.SelectionMode cvSelectionMode;

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
        public SurveyValuesModel selectedResponse;

        public ObservableCollection<SurveyValuesModel> SelectedResponses { get; set; } = new();

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

                if (AllPossibleAnswersList.Any()) AllPossibleAnswersList.Clear();
                AllPossibleAnswersList = await _surveyValuesModelRepository.GetWhereAsync(q => q.SurveyValueType.Equals("Answers"));


                // find the first question in the list and assign it to the current question. Find the last possible question and assign it to LastQuestion.
                // LastQuestion may change..
                if (AllPossibleQuestionsList.Count > 0)
                {
                    CurrentQuestion = AllPossibleQuestionsList[0];
                    LastQuestion = AllPossibleQuestionsList.Last();

                    // set screen values based on properties in CurrentQuestion
                    var setScreenValuesReturn = await SetScreenValues();
                    if (setScreenValuesReturn == 1)
                    {
                        // get answers for currentQuestion
                        var getAnswersForCurrentQuestionReturn = await GetAnswersForCurrentQuestion();
                    }

                    //else
                    //{
                    //    // Show error
                    //    await _messageService.DisplayAlert("Problem", "Problem with current question", "OK", null);
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get list values: {ex.Message}");
                await _messageService.DisplayAlert("Error", "Failed to retrieve survey list values", "OK", null);
            }
            finally
            {
                IsBusy = false;
            }

        }

        

        public async Task<int> SetScreenValues()
        {
            SPID = "987654";

            if (IsFinalQuestion == false)
            {
                ScreenNameLbl = CurrentQuestion.ValueCode;
                CurrentQuestionLbl = CurrentQuestion.ValueText;

                if (CurrentQuestion.Order.Equals(1))
                    LeftBtnLbl = "Cancel";
                else
                    LeftBtnLbl = "Back";

                if (CurrentQuestion.ValueCode.Equals(LastQuestion.ValueCode)) // lastQuestion may change??
                    RightBtnLbl = "Review";
                else
                    RightBtnLbl = "Next";

                if (CurrentQuestion.QuestionType == "List")
                {
                    IsVisibleQuestionTypeList = true;
                    IsVisibleQuestionTypeText = false;
                    IsVisibleAnswerReview = false;

                    if (CurrentQuestion.RuleType == "Multiple")
                    {
                        InstructionLbl = "Select all that apply.";
                        CvSelectionMode = SelectionMode.Multiple;
                    }
                    else
                    {
                        InstructionLbl = "Select one of the following options.";
                        CvSelectionMode = SelectionMode.Single;
                    }
                }
                else if (CurrentQuestion.QuestionType == "Text")
                {
                    IsVisibleQuestionTypeList = false;
                    IsVisibleQuestionTypeText = true;
                    IsVisibleAnswerReview = false;
                }
            }
            else
            {
                ScreenNameLbl = "Review Page";
                CurrentQuestionLbl = "Please review your answers here.";
                LeftBtnLbl = "Back";
                IsVisibleQuestionTypeList = false;
                IsVisibleQuestionTypeText = false;
                IsVisibleAnswerReview = true;
                ScreenNameLbl = "Review Page";
                
            }

            return 1;
        }

        public async Task<int> GetAnswersForCurrentQuestion()
        {
            AnswersForCurrentQuestionList.Clear();
            var itemList = AllPossibleAnswersList.FindAll(t => t.ValueType.Equals(CurrentQuestion.ValueCode));

            foreach (var item in itemList) AnswersForCurrentQuestionList.Add(item);
            return 1;
        }

        [RelayCommand]
        private async void BackButtonClicked()
        {
            Console.WriteLine("BackButtonClicked");
        }


        [RelayCommand]
        public async Task NextButtonClicked()
        {
            Console.WriteLine("NextButtonClicked");

            if (CurrentQuestion != null)
            {
                // Save user question to ActualQuestionsList
                ActualQuestionsList.Add(CurrentQuestion);

                // Save user answer to ActualUserSelectedAnswersList
                if (CurrentQuestion.RuleType.Equals("Single"))
                {
                    if (SelectedResponse == null)
                    {
                        await _messageService.DisplayAlert("", "Please make a selection", "OK", null);
                    }
                    else
                    {
                        int findIndex = ActualUserSelectedAnswersList.FindIndex(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
                        if (findIndex < 0)
                            await SaveSelected();
                        else
                        {
                            responseList.RemoveAt(findIndex);
                            await SaveSelected();
                        }
                    }
                }
                else if (CurrentQuestion.RuleType.Equals("Multiple"))
                {
                    if (SelectedResponses.Count <= 0)
                    {
                        await _messageService.DisplayAlert("", "Please make a selection", "OK", null);
                    }
                    else
                    {
                        SelectedResponse = SelectedResponses[0]; //Getting a value to pull next question

                        var findAllList = ActualUserSelectedAnswersList.FindAll(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
                        if (findAllList.Count == 0)
                        {
                            await SaveSelected();
                        }
                        else
                        {
                            responseList.RemoveAll(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
                            await SaveSelected();
                        }
                    }

                }
                else if (CurrentQuestion.QuestionType.Equals("Text"))
                {

                    if (UserTextAnswer == null)
                    {
                        await _messageService.DisplayAlert("", "Please add your response", "OK", null);
                    }
                    else
                    {
                        int findIndex = ActualUserSelectedAnswersList.FindIndex(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
                        if (findIndex < 0)
                            await SaveSelected();
                        else
                        {
                            responseList.RemoveAt(findIndex);
                            await SaveSelected();
                        }
                    }
                    //await _messageService.DisplayAlert("Text Question", "Add text question here", "OK", null);
                }

                if (IsFinalQuestion == true)
                {

                    Debug.WriteLine("GO TO REVIEW!");
                    var setScreenValuesReturn = await SetScreenValues();
                    if (setScreenValuesReturn == 1)
                    {
                        // get answers for currentQuestion
                        CreateUserResponsesCollection();
                    }

                }
                else
                {
                    // Get new current question

                    var GetCurrentQuestionReturn = await GetCurrentQuestion();
                   
                    if (GetCurrentQuestionReturn == 1)
                    {
                        // set screen values based on properties in CurrentQuestion
                        var setScreenValuesReturn = await SetScreenValues();
                        if (setScreenValuesReturn == 1)
                        {
                            // get answers for currentQuestion
                            var getAnswersForCurrentQuestionReturn = await GetAnswersForCurrentQuestion();
                        }

                    }


                }
            }
        }

        private async Task<int> GetCurrentQuestion()
        {
            CurrentQuestion = AllPossibleQuestionsList.Find(x => x.ValueCode.Equals(SelectedResponse.RuleType));

            return 1;

        }

        [RelayCommand]
        public async Task ResponseChanged(object responseParams)
        {
            SelectedResponses.Clear();
            if (CurrentQuestion.RuleType.Equals("Single"))
            {
                SurveyValuesModel tempResponse = new SurveyValuesModel();
                tempResponse = SelectedResponse;

                // check to see if there are any questions after this answer is added
                if (tempResponse.RuleType.ToLower().Equals("done"))
                {
                    IsFinalQuestion = true;
                }
                else
                {
                    IsFinalQuestion = false;
                }
                SelectedResponses.Add(tempResponse);

            }
            //else (CurrentQuestion.RuleType.Equals("Multiple"))
            else
            {
                List<SurveyValuesModel> myListItems = ((IEnumerable)responseParams).Cast<SurveyValuesModel>().ToList();

                // check to see if there are any questions after this answer is added
                var tempResponse = myListItems.FirstOrDefault(x => x.RuleType.ToLower().Equals("done"));
                if(tempResponse != null)
                {
                    IsFinalQuestion = true;
                }
                else
                {
                    IsFinalQuestion = false;
                }
                


                SelectedResponses = new ObservableCollection<SurveyValuesModel>(myListItems);


                Debug.WriteLine("Count of selected responses in parameter = " + SelectedResponses.Count.ToString());

            }
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
                responseObj.AnswerCode = SelectedResponse.ValueCode;
                responseObj.Id = Id;
                responseObj.AnswerText = SelectedResponse.ValueText;

                ActualUserSelectedAnswersList.Add(responseObj);
            }
            if (CurrentQuestion.RuleType.Equals("Multiple"))
            {
                foreach (var item in SelectedResponses)
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
                foreach (var item in SelectedResponses)
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

