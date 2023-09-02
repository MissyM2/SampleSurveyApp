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
        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        private readonly IUserPreferences _userPreferences;

        private readonly IAsyncRepository<SurveyValuesModel> _surveyValuesModelRepository;
        private readonly IAsyncRepository<SurveyModel> _surveyModelRepository;

        public ObservableCollection<SurveyModel> SurveyList { get; set; }

        [ObservableProperty]
        string selectedListItem;

        [ObservableProperty]
        int id;

        [ObservableProperty]
        string screenName;

        [ObservableProperty]
        string sPID;

        [ObservableProperty]
        string instructionLabel;

        private double width = 0;
        private double height = 0;

        [ObservableProperty]
        bool isPortraitOrientation;

        [ObservableProperty]
        int count;

        [ObservableProperty]
        bool isLandscapeOrientation;

        [ObservableProperty]
        string leftBtnText;

        [ObservableProperty]
        string rightBtnText;

        [ObservableProperty]
        bool isQuestionTypeText;

        [ObservableProperty]
        bool isQuestionTypeList;

        [ObservableProperty]
        Microsoft.Maui.Controls.SelectionMode cvSelectionMode;

        [ObservableProperty]
        SurveyValuesModel currentQuestion;

        List<SurveyValuesModel> surveyQuestionList { get; set; } = new();
        public ObservableCollection<SurveyValuesModel> SurveyListValuesList { get; set; } = new();
        public ObservableCollection<SurveyValuesModel> CurrentSurveyValues { get; set; } = new();
        public ObservableCollection<SurveyValuesModel> AnswerListValues { get; set; } = new();

        // these two properties (1 object, 1 list of objects are for answers selected by user - either single or multiselecct
        [ObservableProperty]
        public SurveyValuesModel selectedResponse;

        public ObservableCollection<SurveyValuesModel> SelectedResponses { get; set; } = new();

        // this  list is for all responses available to user
        public List<SurveyResponseModel> responseList { get; set; } = new();

        [ObservableProperty]
        string currentSurveyQuestion;

        [ObservableProperty]
        string questionType;

        [ObservableProperty]
        string ruleType;

        public SurveyPageVM(
            INavigationService navigationService,
            IMessageService messageService,
            IUserPreferences userPreferences,
            IAsyncRepository<SurveyValuesModel> surveyValuesModelRepository,
            IAsyncRepository<SurveyModel> surveyModelRepository)
        {
            _navigationService = navigationService;
            _messageService = messageService;
            _userPreferences = userPreferences;
            _surveyValuesModelRepository = surveyValuesModelRepository;
            _surveyModelRepository = surveyModelRepository;

            SurveyList = new ObservableCollection<SurveyModel>();

        }



        [RelayCommand]
        public async Task LoadSurveyListValuesList()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (CurrentSurveyValues.Any()) CurrentSurveyValues.Clear();

                // Get question list
                //surveyQuestionList = await _surveyValuesModelRepository.GetWhereAsync(q => q.SurveyValueType.Equals("Questions"));

                var query = _surveyValuesModelRepository.AsQueryable();
                query = query.Where(q => q.SurveyValueType.Equals("Questions"));

                surveyQuestionList = await query.OrderBy(b => b.Order).ToListAsync();


                if (surveyQuestionList.Count > 0)
                {
                    CurrentQuestion = surveyQuestionList[0];

                    LeftBtnText = "Cancel";
                    RightBtnText = "Next";
                    CurrentSurveyQuestion = CurrentQuestion.ValueText;
                    ScreenName = CurrentQuestion.ValueCode;
                    SPID = "987654";

                    if (CurrentQuestion.RuleType == "Multiple")
                    {
                        InstructionLabel = "Select all that apply.";
                        CvSelectionMode = SelectionMode.Multiple;
                    }
                    else
                    {
                        InstructionLabel = "Select one of the following options.";
                        CvSelectionMode = SelectionMode.Single;
                    }


                    if (CurrentQuestion.QuestionType == "List")
                    {
                        IsQuestionTypeList = true;
                        IsQuestionTypeText = false;
                    }
                    else
                    {
                        IsQuestionTypeList = false;
                        IsQuestionTypeText = true;
                    }

                    // Get Answers
                    AnswerListValues.Clear();
                    var query1 = _surveyValuesModelRepository.AsQueryable();
                    query1 = query1.Where(t => t.SurveyValueType.Equals("Answers") && t.ValueType.Equals(CurrentQuestion.ValueType));

                    List<SurveyValuesModel> answerList = await query.OrderBy(b => b.Order).ToListAsync();

                    //List<SurveyValuesModel> answerList = await _surveyValuesModelRepository.GetSurveyAnswersForAQuestionAsync(string valueType)

                    //    return await _dbConnection.Table<SurveyValuesModel>().Where((t => t.SurveyValueType.Equals("Answers") && t.ValueType.Equals(valueType);


                    foreach (var anserValue in answerList)
                    {
                        AnswerListValues.Add(anserValue);
                    }

                }
                else
                {
                    // Show error
                    await _messageService.DisplayAlert("HEC", "Problem with current HEC Question", "OK", null);
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
                if (CurrentQuestion.RuleType.Equals("Single"))
                {
                    if (SelectedResponse == null)
                    {
                        await _messageService.DisplayAlert("", "Please make a selection", "OK", null);
                    }
                    else
                    {
                        int findIndex = responseList.FindIndex(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
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

                        var findAllList = responseList.FindAll(v => v.QuestionCode.Equals(CurrentQuestion.ValueCode));
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
                    SelectedResponse = AnswerListValues[0];
                    //await _messageService.DisplayAlert("Text Question", "Add text question here", "OK", null);
                }

                if (SelectedResponse.RuleType.ToLower().Equals("done"))
                {

                    await Shell.Current.GoToAsync($"{nameof(SurveyReviewPageVM)}",
                        new Dictionary<string, object>
                        {
                            ["ResponseList"] = responseList
                        });

                }
                else
                {
                    //GET NEW CURRENT QUESTION
                    //CurrentQuestion = surveyQuestionList.Find(x => x.ValueType.Equals(SelectedResponse.RuleType));
                    CurrentQuestion = surveyQuestionList.Find(x => x.ValueCode.Equals(SelectedResponse.RuleType));


                    // SET NAVIGATION BUTTONS
                    if (CurrentQuestion.Order == 1)
                        LeftBtnText = "Cancel";
                    else
                        LeftBtnText = "Back";
                    // NEXT BUTTON
                    if (CurrentQuestion.ValueCode.Equals("HEC7"))
                        RightBtnText = "Review";
                    else
                        RightBtnText = "Next";

                    // UPDATE SCREEN
                    CurrentSurveyQuestion = CurrentQuestion.ValueText;
                    //ScreenName = CurrentQuestion.ValueType;
                    ScreenName = CurrentQuestion.ValueCode;
                    SPID = "987654";
                    QuestionType = CurrentQuestion.QuestionType;

                    if (CurrentQuestion.RuleType == "Multiple")
                    {
                        InstructionLabel = "Select all that apply.";
                        CvSelectionMode = SelectionMode.Multiple;
                    }
                    else
                    {
                        InstructionLabel = "Select one of the following options.";
                        CvSelectionMode = SelectionMode.Single;
                    }


                    // RENDER SCREEN BASED ON QUESTION TYPE
                    if (QuestionType == "List")
                    {
                        IsQuestionTypeList = true;
                        IsQuestionTypeText = false;
                    }
                    else
                    {
                        IsQuestionTypeList = false;
                        IsQuestionTypeText = true;
                    }

                    // Get Answers
                    AnswerListValues.Clear();

                    var query2 = _surveyValuesModelRepository.AsQueryable();
                    query2 = query2.Where(t => t.SurveyValueType.Equals("Answers") && t.ValueType.Equals(CurrentQuestion.ValueType));

                    List<SurveyValuesModel> answerList = await query2.OrderBy(b => b.Order).ToListAsync();

                    //surveyQuestionList = await query.OrderBy(b => b.Order).ToListAsync();
                    //List<SurveyValuesModel> answerList = await _surveysDatabaseHelper.GetSurveyAnswersForAQuestionAsync(CurrentQuestion.ValueCode);
                    foreach (var anserValue in answerList)
                    {
                        AnswerListValues.Add(anserValue);
                    }
                }
            }
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
            else if (CurrentQuestion.RuleType.Equals("Multiple"))
            {
                List<SurveyValuesModel> myListItems = ((IEnumerable)responseParams).Cast<SurveyValuesModel>().ToList();
                SelectedResponses = new ObservableCollection<SurveyValuesModel>(myListItems);
                Debug.WriteLine("Count of selected responses in parameter = " + SelectedResponses.Count.ToString());

            }
            else if (CurrentQuestion.RuleType.Equals("Text"))
            {
                await _messageService.DisplayAlert("Text Question", "Add text question here", "OK", null);
            }

        }

        public async Task SaveSelected()
        {

            if (CurrentQuestion.RuleType.Equals("Single"))
            {
                SurveyResponseModel responseObj = new SurveyResponseModel();
                responseObj.QuestionCode = CurrentQuestion.ValueCode;
                responseObj.QuestionText = CurrentQuestion.ValueText;
                responseObj.AnswerCode = SelectedResponse.ValueCode;
                responseObj.Id = Id;
                responseObj.AnswerText = SelectedResponse.ValueText;

                responseList.Add(responseObj);
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

                    responseList.Add(responseObj);
                }
            }

            //foreach(var item in responseList)
            //{
            //    await _databaseHelper.InsertData(item);
            //}
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
    }
}

