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
                // var queryAnswers = _surveyValuesModelRepository.AsQueryable();
                AllPossibleAnswersList = await _surveyValuesModelRepository.GetWhereAsync(q => q.SurveyValueType.Equals("Answers"));

                //AllPossibleQuestionsList = await _surveyValuesModelRepository.OrderBy(b => b.Order).ToListAsync();


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

            if (IsVisibleAnswerReview == false)
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
            //if (CurrentQuestion != null)
            //{
            //    if (CurrentQuestion.RuleType.Equals("Single"))
            //    {
            //        if (SelectedResponse == null)
            //        {
            //            await _messageService.DisplayAlert("", "Please make a selection", "OK", null);
            //        }
            //        else
            //        {
            //            int findIndex = responseList.FindIndex(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
            //            if (findIndex < 0)
            //                await SaveSelected();
            //            else
            //            {
            //                responseList.RemoveAt(findIndex);
            //                await SaveSelected();
            //            }
            //        }
            //    }
            //    else if (CurrentQuestion.RuleType.Equals("Multiple"))
            //    {
            //        if (SelectedResponses.Count <= 0)
            //        {
            //            await _messageService.DisplayAlert("", "Please make a selection", "OK", null);
            //        }
            //        else
            //        {
            //            SelectedResponse = SelectedResponses[0]; //Getting a value to pull next question

            //            var findAllList = responseList.FindAll(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
            //            if (findAllList.Count == 0)
            //            {
            //                await SaveSelected();
            //            }
            //            else
            //            {
            //                responseList.RemoveAll(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
            //                await SaveSelected();
            //            }
            //        }

            //    }
            //    else if (CurrentQuestion.QuestionType.Equals("Text"))
            //    {
            //        SelectedResponse = AnswerListValues[0];
            //        //await _messageService.DisplayAlert("Text Question", "Add text question here", "OK", null);
            //    }

            //    if (SelectedResponse.RuleType.ToLower().Equals("done"))
            //    {

            //        await Shell.Current.GoToAsync($"{nameof(SurveyReviewPageVM)}",
            //            new Dictionary<string, object>
            //            {
            //                ["ResponseList"] = responseList
            //            });

            //    }
            //    else
            //    {
            //        //GET NEW CURRENT QUESTION
            //        //CurrentQuestion = surveyQuestionList.Find(x => x.ValueType.Equals(SelectedResponse.RuleType));
            //        CurrentQuestion = surveyQuestionList.Find(x => x.ValueCode.Equals(SelectedResponse.RuleType));


            //        // SET NAVIGATION BUTTONS
            //        if (CurrentQuestion.Order == 1)
            //            LeftBtnLbl = "Cancel";
            //        else
            //            LeftBtnLbl = "Back";
            //        // NEXT BUTTON
            //        if (CurrentQuestion.ValueCode.Equals("HEC7"))
            //            RightBtnLbl = "Review";
            //        else
            //            RightBtnLbl = "Next";

            //        // UPDATE SCREEN
            //        CurrentSurveyQuestion = CurrentQuestion.ValueText;
            //        //ScreenName = CurrentQuestion.ValueType;
            //        ScreenName = CurrentQuestion.ValueCode;
            //        SPID = "987654";
            //        QuestionType = CurrentQuestion.QuestionType;

            //        if (CurrentQuestion.RuleType == "Multiple")
            //        {
            //            InstructionLbl = "Select all that apply.";
            //            CvSelectionMode = SelectionMode.Multiple;
            //        }
            //        else
            //        {
            //            InstructionLbl = "Select one of the following options.";
            //            CvSelectionMode = SelectionMode.Single;
            //        }


            //        // RENDER SCREEN BASED ON QUESTION TYPE
            //        if (QuestionType == "List")
            //        {
            //            IsVisibleQuestionTypeList = true;
            //            IsVisibleQuestionTypeText = false;
            //        }
            //        else
            //        {
            //            IsVisibleQuestionTypeList = false;
            //            IsVisibleQuestionTypeText = true;
            //        }

            //        // Get Answers
            //        AnswerListValues.Clear();

            //        var query2 = _surveyValuesModelRepository.AsQueryable();
            //        query2 = query2.Where(t => t.SurveyValueType.Equals("Answers") && t.ValueType.Equals(CurrentQuestion.ValueType));

            //        List<SurveyValuesModel> answerList = await query2.OrderBy(b => b.Order).ToListAsync();

            //        //surveyQuestionList = await query.OrderBy(b => b.Order).ToListAsync();
            //        //List<SurveyValuesModel> answerList = await _surveysDatabaseHelper.GetSurveyAnswersForAQuestionAsync(CurrentQuestion.ValueCode);
            //        foreach (var anserValue in answerList)
            //        {
            //            AnswerListValues.Add(anserValue);
            //        }
            //    }
            //}
        }

        [RelayCommand]
        public async Task ResponseChanged(object responseParams)
        {
            SelectedResponses.Clear();
            if (CurrentQuestion.RuleType.Equals("Single"))
            {
                SurveyValuesModel tempResponse = new SurveyValuesModel();
                tempResponse = SelectedResponse;
                SelectedResponses.Add(tempResponse);

            }
            //else (CurrentQuestion.RuleType.Equals("Multiple"))
            else
            {
                List<SurveyValuesModel> myListItems = ((IEnumerable)responseParams).Cast<SurveyValuesModel>().ToList();
                SelectedResponses = new ObservableCollection<SurveyValuesModel>(myListItems);
                Debug.WriteLine("Count of selected responses in parameter = " + SelectedResponses.Count.ToString());

            }
            //else if (CurrentQuestion.RuleType.Equals("Text"))
            //{
            //    await _messageService.DisplayAlert("Text Question", "Add text question here", "OK", null);
            //}

        }

        //public async Task SaveSelected()
        //{

        //    if (CurrentQuestion.RuleType.Equals("Single"))
        //    {
        //        SurveyResponseModel responseObj = new SurveyResponseModel();
        //        responseObj.QuestionCode = CurrentQuestion.ValueCode;
        //        responseObj.QuestionText = CurrentQuestion.ValueText;
        //        responseObj.AnswerCode = SelectedResponse.ValueCode;
        //        responseObj.Id = Id;
        //        responseObj.AnswerText = SelectedResponse.ValueText;

        //        responseList.Add(responseObj);
        //    }
        //    if (CurrentQuestion.RuleType.Equals("Multiple"))
        //    {
        //        foreach (var item in SelectedResponses)
        //        {
        //            SurveyResponseModel responseObj = new SurveyResponseModel();
        //            responseObj.QuestionCode = CurrentQuestion.ValueCode;
        //            responseObj.QuestionText = CurrentQuestion.ValueText;
        //            responseObj.Id = Id;
        //            responseObj.AnswerCode = item.ValueCode;
        //            responseObj.AnswerText = item.ValueText;

        //            responseList.Add(responseObj);
        //        }
        //    }

        //    //foreach(var item in responseList)
        //    //{
        //    //    await _databaseHelper.InsertData(item);
        //    //}
        //}


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

