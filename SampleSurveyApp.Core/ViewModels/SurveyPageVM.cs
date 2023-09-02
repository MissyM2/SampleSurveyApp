using System;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SampleSurveyApp.Core.ViewModels.Base;
using SampleSurveyApp.Core.Services;
using SampleSurveyApp.Core.Database;
using SampleSurveyApp.Core.Domain;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SampleSurveyApp.Core.ViewModels
{
	public partial class SurveyPageVM : BaseVM
	{
        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        private readonly IUserPreferences _userPreferences;

        private readonly IRepository<SurveyValuesModel> _surveyValuesModelRepository;
        private readonly IRepository<SurveyModel> _surveyModelRepository;

        public ObservableCollection<SurveyModel> SurveyList { get; set; }

        [ObservableProperty]
        string selectedListItem;

        public SurveyPageVM(
            INavigationService navigationService,
            IMessageService messageService,
            IUserPreferences userPreferences,
            IRepository<SurveyValuesModel> surveyValuesModelRepository,
            IRepository<SurveyModel> surveyModelRepository)
        {
            _navigationService = navigationService;
            _messageService = messageService;
            _userPreferences = userPreferences;
            _surveyValuesModelRepository = surveyValuesModelRepository;
            _surveyModelRepository = surveyModelRepository;

            SurveyList = new ObservableCollection<SurveyModel>();

        }

       

        //        [RelayCommand]
        //        public async Task GetLogins()
        //        {
        //#if DEBUG
        //            await Task.Delay(500);
        //#endif

        //            LoginList.Clear();

        //            var items = await _surveyValuesModelRepository.GetAllAsync();

        //            foreach (var item in items) LoginList.Add(item);
        //        }

        //        [RelayCommand]
        //        public async Task DeleteAllLogins()
        //        {
        //            await _surveyValuesModelRepository.DeleteAllAsync();

        //        }

        //        [RelayCommand]
        //        public async Task<SurveyModel> GetLoginById(int id)
        //        {
        //            var selectedListItem = await _surveyValuesModelRepository.GetById(id);
        //            return selectedListItem;

        //        }

        //        [RelayCommand]
        //        public void GoToDetails()
        //        {
        //            Console.WriteLine("Go to getils");

        //        }




        //[RelayCommand]
        //public async Task<UserModel> GetLoginById(int id)
        //{
        //    var selectedLogin = await _loginModelRepository.GetById(id);
        //    return selectedLogin;

        //}

        //[RelayCommand]
        //public async Task<User> GetUserByUsername(string username)
        //{
        //    return await _userService.GetByUsername(username);

        //}

        //[RelayCommand]
        //public async Task<User> GetUserByEmailAddress(string emailAddress)
        //{
        //    return await _userService.GetByEmail(emailAddress);

        //}




    }
}

