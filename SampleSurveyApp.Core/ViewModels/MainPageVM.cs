using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.Services;
using SampleSurveyApp.Core.ViewModels.Base;

namespace SampleSurveyApp.Core.ViewModels
{
	public partial class MainPageVM : BaseVM
	{
        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        private readonly IUserPreferences _userPreferences;

        private readonly IAsyncRepository<SurveyValuesModel> _surveyValuesModelRepository;
        private readonly IAsyncRepository<SurveyModel> _surveyModelRepository;
        private readonly IAsyncRepository<SurveyResponseModel> _surveyResponseModelRepository;

        [ObservableProperty]
        public SurveyModel newSurvey;

        [ObservableProperty]
        public SurveyModel insertedSurvey;

        public ObservableCollection<SurveyModel> SurveyList { get; set; } = new();
        public ObservableCollection<SurveyResponseModel> SurveyResponseList { get; set; } = new();

        public MainPageVM(
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
        public async Task Refresh()
        {
            IsBusy = true;


#if DEBUG
            await Task.Delay(500);
#endif

            var asd = new AddSurveyData(_surveyValuesModelRepository);
            await asd.AddSurveyValuesAsync();


            IsBusy = false;

            //toaster?.MakeToast("Refreshed!");
        }

        [RelayCommand]
        public async Task CreateSurvey()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                NewSurvey = new SurveyModel();
                NewSurvey.SurveyDate = DateTime.Now;
                NewSurvey.SurveyStatus = "I";
                NewSurvey.SyncStatus = "I";


                    // insert a new record
                await _surveyModelRepository.InsertAsync(NewSurvey);
                InsertedSurvey = NewSurvey;

                //await _navigationService.PushAsync<SurveyPageVM>($"itemID={InsertedSurvey.ItemID}");
               

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to insert a survey: {ex.Message}");
                await _messageService.DisplayAlert("Error", "Failed to insert a surveys", "OK", null);

            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public async Task DeleteAllSurveys()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                if (SurveyList.Any()) SurveyList.Clear();
                var surveys = new List<SurveyModel>();

                surveys = await _surveyModelRepository.GetAllAsync();
                foreach (var survey in surveys)
                {
                    await _surveyModelRepository.DeleteAsync(survey);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to delete all surveys: {ex.Message}");
                await _messageService.DisplayAlert("Error", "Failed to delete all surveys", "OK", null);

            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public async Task DeleteAllResponses()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                if (SurveyResponseList.Any()) SurveyResponseList.Clear();
                var responses = new List<SurveyResponseModel>();
                responses = await _surveyResponseModelRepository.GetAllAsync();
                foreach (var response in responses)
                {
                    await _surveyResponseModelRepository.DeleteAsync(response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to delete all responses: {ex.Message}");
                await _messageService.DisplayAlert("Error", "Failed to delete all responses", "OK", null);

            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public async Task ViewSurveyList()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (SurveyList.Any()) SurveyList.Clear();
                var surveys = new List<SurveyModel>();

                surveys = await _surveyModelRepository.GetAllAsync();

                foreach (var survey in surveys) SurveyList.Add(survey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get cars: {ex.Message}");
                await _messageService.DisplayAlert("Error", "Failed to retrieve surveys", "OK", null);

            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        public async Task ViewResponseList()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                if (SurveyResponseList.Any()) SurveyResponseList.Clear();
                var surveyResponseValues = new List<SurveyResponseModel>();
                surveyResponseValues = await _surveyResponseModelRepository.GetAllAsync();

                foreach (var surveyResponseValue in surveyResponseValues) SurveyResponseList.Add(surveyResponseValue);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get cars: {ex.Message}");
                await _messageService.DisplayAlert("Error", "Failed to retrieve survey list values", "OK", null);

            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}

