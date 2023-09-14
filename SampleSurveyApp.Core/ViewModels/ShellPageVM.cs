using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SampleSurveyApp.Core.Services;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.ViewModels.Base;
using System.Diagnostics;

namespace SampleSurveyApp.Core.ViewModels
{
	public partial class ShellPageVM : BaseVM
	{

        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;

        private readonly IAsyncRepository<SurveyModel> _surveyModelRepository;
        private readonly IAsyncRepository<SurveyResponseModel> _surveyResponseModelRepository;

        [ObservableProperty]
        public SurveyModel newSurvey;

        [ObservableProperty]
        public SurveyModel insertedSurvey;

        public ObservableCollection<SurveyModel> SurveyList { get; set; } = new();
        public ObservableCollection<SurveyResponseModel> SurveyResponseList { get; set; } = new();

        public ShellPageVM(
            INavigationService navigationService,
            IMessageService messageService,
            IAsyncRepository<SurveyModel> surveyModelRepository,
            IAsyncRepository<SurveyResponseModel> surveyResponseModelRepository)
        {
            _navigationService = navigationService;
            _messageService = messageService;
            _surveyModelRepository = surveyModelRepository;
            _surveyResponseModelRepository = surveyResponseModelRepository;
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
                await _messageService.DisplayAlert("Error", "Failed to delete all surveys", "OK", "Cancel");

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
                Debug.WriteLine($"Unable to delete all surveys: {ex.Message}");
                await _messageService.DisplayAlert("Error", "Failed to delete all responses", "OK", "Cancel");

            }
            finally
            {
                IsBusy = false;
            }
        }       

    }
}

