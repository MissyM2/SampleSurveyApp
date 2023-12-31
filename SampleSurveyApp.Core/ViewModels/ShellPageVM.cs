﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SampleSurveyApp.Core.Services;
using SampleSurveyApp.Core.Domain;
using SampleSurveyApp.Core.ViewModels.Base;
using System.Diagnostics;
using SampleSurveyApp.Core.Localization;
using System.Globalization;

namespace SampleSurveyApp.Core.ViewModels
{
    public partial class ShellPageVM : BaseVM
	{

        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        private readonly CultureManager _cultureManager;

        public LocalizationResourceManager LocalizationResourceManager => LocalizationResourceManager.Instance;

        private readonly IAsyncRepository<SurveyModel> _surveyModelRepository;
        private readonly IAsyncRepository<SurveyResponseModel> _surveyResponseModelRepository;

        [ObservableProperty]
        public SurveyModel newSurvey;

        [ObservableProperty]
        public SurveyModel insertedSurvey;

        [ObservableProperty]
        string currCulture;

        [ObservableProperty]
        string currCultureString;

        [ObservableProperty]
        string currDate;

        public ObservableCollection<SurveyModel> SurveyList { get; set; } = new();
        public ObservableCollection<SurveyResponseModel> SurveyResponseList { get; set; } = new();

        public ShellPageVM(
            INavigationService navigationService,
            IMessageService messageService,
            CultureManager cultureManager,
            IAsyncRepository<SurveyModel> surveyModelRepository,
            IAsyncRepository<SurveyResponseModel> surveyResponseModelRepository)
        {
            _navigationService = navigationService;
            _messageService = messageService;
            _cultureManager = cultureManager;
            _surveyModelRepository = surveyModelRepository;
            _surveyResponseModelRepository = surveyResponseModelRepository;

            CurrDate = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern, CultureInfo.CurrentCulture);

            /*CultureInfo ci = CultureInfo.CurrentCulture;
            CurrDate = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern, ci);
*/
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
                var deleteSurveys = await _messageService.DisplayAlert(AppResources.SurveyCountMsgTitle, AppResources.CountMsg1 + " " + surveys.Count + AppResources.CountMsg2, "OK", AppResources.CancelMsg);
                if (deleteSurveys == true)
                {
                    foreach (var survey in surveys)
                    {
                        await _surveyModelRepository.DeleteAsync(survey);
                    }
                }
                else
                {
                    return;
                }
                surveys = await _surveyModelRepository.GetAllAsync();
                await _messageService.CustomAlert(AppResources.SurveysDeletedMsgTitle, surveys.Count + " " + AppResources.SurveysDeletedMsg, "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ShellPageVM.cs:DeleteAllSurveys: Unable to delete all surveys: {ex.Message}");
                await _messageService.CustomAlert("Error", "Failed to delete all surveys", "OK");
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
                var deleteResponses = await _messageService.DisplayAlert(AppResources.ResponseCountMsgTitle, AppResources.CountMsg1 + " " + responses.Count + AppResources.CountMsg2, "OK", AppResources.CancelMsg);

                responses = await _surveyResponseModelRepository.GetAllAsync();
                foreach (var response in responses)
                {
                    await _surveyResponseModelRepository.DeleteAsync(response);
                }

                responses = await _surveyResponseModelRepository.GetAllAsync();
                await _messageService.CustomAlert(AppResources.ResponsesDeletedMsgTitle, responses.Count + " " + AppResources.ResponsesDeletedMsg, "OK");


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ShellPageVM.cs:DeleteAllResponses: Unable to delete all responses: {ex.Message}");
                await _messageService.CustomAlert("Error", "Failed to delete all responses", "OK");

            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task GetLanguage()
        {
            try
            {
                Debug.WriteLine("GetLanguage: Culture of {0} in application domain {1}: {2}",
                          Thread.CurrentThread.Name,
                          AppDomain.CurrentDomain.FriendlyName,
                          CultureInfo.CurrentCulture.Name);


                CurrCultureString = "Lang " + CultureInfo.CurrentCulture.DisplayName;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ShellPageVM.cs:GetLanguage: '{ex}'");
            }
            

        }

        [RelayCommand]
        public async Task ChangeLanguage()
        {
            try
            {
                await _cultureManager.ChangeLang(CultureInfo.CurrentCulture.Name);
                CurrCultureString = "Lang: " + CultureInfo.CurrentCulture.DisplayName;

                //CultureInfo ci = CultureInfo.CurrentCulture;
                CurrDate = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern, CultureInfo.CurrentCulture);


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ShellPageVM.cs:ChangeLanguage: '{ex}'");
            }
            


        }


    }
}

