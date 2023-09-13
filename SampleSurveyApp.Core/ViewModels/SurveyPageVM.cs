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
        public ObservableCollection<SurveyAnswerModel> UserSelectedAnswers { get; set; } = new();

        [ObservableProperty]
        public string userTextAnswer;

        [ObservableProperty]
        SurveyQuestionModel currentQuestion;

        //public ObservableCollection<SurveyAnswerModel> AnswerOptionsForCurrentQuestionCollection { get; set; }
        public IList<SurveyAnswerModel> AnswerOptionsForCurrentQuestionCollection { get; set; }


        [ObservableProperty]
        int currQuestionIndex;
        
        [ObservableProperty]
        string currQCode;

        [ObservableProperty]
        SurveyQuestionModel lastQuestion;

        [ObservableProperty]
        string nextQCode;

        [ObservableProperty]
        string prevQCode;

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
        string screenNameLbl;

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
        bool isVisibleSurveyHeader = true;

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

        //[ObservableProperty]
        //string selectedListItem;

        [ObservableProperty]
        string selectedItem;

        [ObservableProperty]
        bool answerIsSelected;

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
        bool isReview = false;

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
            await Refresh();
            IsBusy = false;

        }



        #region Navigation
        [RelayCommand]
        public async Task Refresh()
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
                if (answer.QType == CurrentQuestion.QCode)
                {
                    AnswerOptionsForCurrentQuestionCollection.Add(answer);
                }
            }
            // update question in q collection
            var foundQ = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode.Equals(CurrentQuestion.QCode));
            foundQ.IsSelected = true;
            foundQ.prevQCode = "";

                // do i need this  
            CurrQCode = CurrentQuestion.QCode;

            IsVisibleSurveyHeader = false;

                // set screen values based on properties in CurrentQuestion
             SetScreenValuesOnOpen();

        }

        [RelayCommand]
        public async Task NextButtonClicked()
        {
            Console.WriteLine("NextButtonClicked");
            if (CurrentQuestion.nextQCode == "")
            {
                // this must be review page
             }
            else
            {
                // determine which kind of answer it is and mark it as selected

                if (CurrentQuestion.QType == "List")
                {
                    if (CurrentQuestion.RuleType.Equals("Single"))
                    {
                        if (UserSelectedAnswer == null || string.IsNullOrEmpty(UserSelectedAnswer.AText))
                        {
                            await _messageService.DisplayAlert("", "Please make a selection", "OK", "Cancel");
                        }
                        else
                        {
                            var foundA = AllPossibleAnswerOptionsCollection.FirstOrDefault(x => x.QType == CurrentQuestion.QCode && x.AText == UserSelectedAnswer.AText);
                            if (foundA != null)
                            {
                                foundA.IsSelected = true;
                            }
                            else
                            {
                                Debug.WriteLine("NextButtonClicked: Problem:There was no option selected.");
                            }

                        }
                    }
                    else  // (CurrentQuestion.RuleType.Equals("Multiple"))
                    {
                        if (UserSelectedAnswers.Count <= 0)
                        {
                            await _messageService.DisplayAlert("", "Please make a selection", "OK", "Cancel");
                        }
                        else
                        {

                            foreach (var foundA in AllPossibleAnswerOptionsCollection)
                            {
                                foreach (var answerOptionSelected in UserSelectedAnswers)
                                {
                                    if (foundA.QType == answerOptionSelected.QType && foundA.AText == answerOptionSelected.AText)
                                    {
                                        foundA.IsSelected = true;
                                    }
                                }
                            }
                        }
                    }
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

                        var foundA = AllPossibleAnswerOptionsCollection.FirstOrDefault(x => x.QType == CurrentQuestion.QCode && x.AText == "");
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

                // update current q with nextq and update nextq property

                var foundCurrQ = AllPossibleQuestionsCollection.FirstOrDefault(v => v.QCode.Equals(CurrentQuestion.QCode));

                if (UserSelectedAnswer.RuleType == null || UserSelectedAnswer.RuleType == "DONE")  // this is the last q
                {
                    Debug.WriteLine("NextButtonClicked: this is the last question, go to review");
                    // go to review
                    CurrentQuestion.nextQCode = "";
                    CreateUserResponsesCollection();

                    SetScreenValuesOnOpen();
                }
                var foundNextQ = AllPossibleQuestionsCollection.FirstOrDefault(v => v.QCode.Equals(UserSelectedAnswer.RuleType));

                if (foundNextQ == null)
                {
                    //  there are no more questions
                    CurrentQuestion = null;
                }
                else
                {
                    CurrentQuestion.nextQCode = foundNextQ.QCode;
                    // set prev q
                    if (CurrentQuestion.QCode == "Q1")  // this is the first q
                    {
                        foundCurrQ.prevQCode = "";
                    }
                    foundNextQ.prevQCode = foundCurrQ.QCode;

                    // all prev codes have been updated.

                    // update current question
                    CurrentQuestion = foundNextQ;

                    // update question in q collection
                    foundCurrQ = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode.Equals(CurrentQuestion.QCode));
                    foundCurrQ.IsSelected = true;

                    // get answers for current q
                    AnswerOptionsForCurrentQuestionCollection.Clear();
                    foreach (var answer in answerSource)
                    {
                        if (answer.QType == CurrentQuestion.QCode)
                        {
                            AnswerOptionsForCurrentQuestionCollection.Add(answer);
                        }
                    }
                }

            }


            // set screen values based on properties in CurrentQuestion
            SetScreenValuesOnOpen();
            


        }


        [RelayCommand]
        private async void BackButtonClicked()
        {
            Console.WriteLine("BackButtonClicked");

            // see if it is review page
            if (CurrentQuestion == null)
            {
                var foundQs = AllPossibleQuestionsCollection.Where(x => x.IsSelected.Equals(true));
                CurrentQuestion = foundQs.Last();
            }
            else
            {
                //get new curr q from prev q
                CurrentQuestion = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode.Equals(CurrentQuestion.prevQCode));

            }

            // get answers for curr q
            AnswerOptionsForCurrentQuestionCollection.Clear();
            foreach (var answer in answerSource)
            {
                if (answer.QType == CurrentQuestion.QCode)
                {
                    AnswerOptionsForCurrentQuestionCollection.Add(answer);
                }
            }

            if (CurrentQuestion.QType == "List")
            {
                if (CurrentQuestion.RuleType == "Single")
                {
                    var selectedAnswer = AnswerOptionsForCurrentQuestionCollection.FirstOrDefault(x => x.QType == CurrentQuestion.QCode && x.IsSelected == true);
                }
                else if (CurrentQuestion.RuleType == "Multiple")
                {
                    var selectedAnswers = AnswerOptionsForCurrentQuestionCollection.Where(t => t.QType == CurrentQuestion.QCode && t.IsSelected == true);
                }
            }

            else  // q type must be text
            {
                Debug.WriteLine("Get existing text for back button");
            }

            SetScreenValuesOnOpen();

        }



        #endregion


        #region UI

        public void SetScreenValuesOnOpen()
        {
            SPID = "987654";

            if (CurrentQuestion != null)
            {
                if (RightBtnLbl != "Review")
                {
                    IsWorkingRightBtn = true;

                    ScreenNameLbl = CurrentQuestion.QCode;
                    CurrentQuestionLbl = CurrentQuestion.QText;
                    if (CurrentQuestion.prevQCode != "")
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
                    IsVisibleAnswerReview = false;

                    if (CurrentQuestion.QType == "List")
                    {
                        if (CurrentQuestion.RuleType == "Single")
                        {
                            IsVisibleRuleTypeSingle = true;
                            IsVisibleRuleTypeMultiple = false;
                            IsVisibleAnswerReview = false;
                            IsVisibleQTypeText = false;
                            InstructionLbl = "SINGLE Select an option.";
                        }
                        else // CurrentQuestion.RuleType must be multiple
                        {
                            IsVisibleRuleTypeSingle = false;
                            IsVisibleRuleTypeMultiple = true;
                            IsVisibleAnswerReview = false;
                            IsVisibleQTypeText = false;
                            InstructionLbl = "MULTIPLE: Select all that apply.";
                        }

                    }
                    else // CurrentQuestion.QType can only be Text
                    {
                        IsVisibleRuleTypeSingle = false;
                        IsVisibleRuleTypeMultiple = false;
                        IsVisibleQTypeText = true;
                        IsVisibleAnswerReview = false;
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
                    IsVisibleQTypeText = false;
                    IsVisibleAnswerReview = true;
                }
            }
        }

        #endregion

        #region Answers

        private int GetAnswerOptionsForCurrentQuestion()
        {
            AnswerOptionsForCurrentQuestionCollection.Clear();
            var filteredList = AllPossibleAnswerOptionsCollection.Where(t => t.QType.Equals(CurrentQuestion.QCode));

            AnswerOptionsForCurrentQuestionCollection = new ObservableCollection<SurveyAnswerModel>(filteredList);

            //foreach (var item in itemList) AnswerOptionsForCurrentQuestionList.Add(item);
            return 1;
        }

        #endregion

        //[RelayCommand]
        //public async Task ChangeStatus()
        //{
        //    var filteredList = AllPossibleAnswerOptionsCollection.Where(x => x.QType.Equals(UserSelectedAnswer.QType) && x.ACode.Equals(UserSelectedAnswer.ACode));

        //    if (filteredList.Count() == 1)
        //    {
        //        if (filteredList.First().IsSelected == true)
        //        {
        //            filteredList.First().IsSelected = false;
        //            IsSelected = false;
        //        }
        //        else
        //        {
        //            filteredList.First().IsSelected = true;
        //            IsSelected = true;
        //        }
        //    }
           
        //}



        [RelayCommand]
        public async Task SingleAnswerSelected()
        {

            var filteredList = AllPossibleAnswerOptionsCollection.Where(x => x.QType.Equals(UserSelectedAnswer.QType) && x.ACode.Equals(UserSelectedAnswer.ACode));

            if (filteredList.Count() == 1)
            {
                if (filteredList.First().IsSelected == true)
                {
                    filteredList.First().IsSelected = false;

                    IsSelected = false;
                }
                else
                {
                    filteredList.First().IsSelected = true;
                    var sa = AnswerOptionsForCurrentQuestionCollection.FirstOrDefault(x => x.QType.Equals(UserSelectedAnswer.QType) && x.ACode.Equals(UserSelectedAnswer.ACode));
                    sa.IsSelected = true;
                    IsSelected = true;
                }
            }

            // Check to see if this is the last question
            if (UserSelectedAnswer.RuleType.ToLower().Equals("done"))
            {
                NextQCode = null;
                RightBtnLbl = "Review";
                NextQCode = "";
            }
            else
            {
                NextQCode = UserSelectedAnswer.RuleType;
            }
            
            
        }

        [RelayCommand]
        public async Task ResponseChanged(object responseParams)
        {
            UserSelectedAnswers.Clear();

            List<SurveyAnswerModel> myListItems = ((IEnumerable)responseParams).Cast<SurveyAnswerModel>().ToList();

            // check to see if there are any questions after this answer is added
            var tempResponse = myListItems.FirstOrDefault(x => x.RuleType.ToLower().Equals("done"));
            if (tempResponse != null)
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

        private void CreateUserResponsesCollection()
        {
            UserAnswerGroups.Clear();

            var dict = AllPossibleAnswerOptionsCollection.Where(x => x.IsSelected.Equals(true)).GroupBy(o => o.QType)
                .ToDictionary(g => g.Key, g => g.ToList());

            //var dict = ActualUserSelectedAnswersList.GroupBy(o => o.QCode)
            //    .ToDictionary(g => g.Key, g => g.ToList());

            foreach (KeyValuePair<string, List<SurveyAnswerModel>> item in dict)
            {
                        
                var q = AllPossibleQuestionsCollection.FirstOrDefault(x => x.QCode == item.Key);
                UserAnswerGroups.Add(new AnswerGroup(item.Key, q.QText, new List<SurveyAnswerModel>(item.Value)));
            }
        }




        public class AnswerGroup : List<SurveyAnswerModel>
        {
            public string QCode { get; set; }
            public string QText { get; set; }

            //public AnswerGroup(string qCode, string qText, List<SurveyResponseModel> userResponses) : base(userResponses)
            public AnswerGroup(string qCode, string qText, List<SurveyAnswerModel> userResponses) : base(userResponses)
            {
                QCode = qCode;
                QText = qText;
            }
        }

    }
}

