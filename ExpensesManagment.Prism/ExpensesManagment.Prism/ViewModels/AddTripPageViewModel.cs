using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
using ExpensesManagment.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Prism.ViewModels
{
    public class AddTripPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IRegexHelper _regexHelper;
        private TripRequest _trip;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _addCommand;

        public AddTripPageViewModel(INavigationService navigationService,
            IApiService apiService,
            IRegexHelper regexHelper) : base(navigationService)
        {
            Title = Languages.AddTrip;
            _navigationService = navigationService;
            _apiService = apiService;
            _regexHelper = regexHelper;
            Trip = new TripRequest
            {
                StartDate = DateTime.UtcNow.ToLocalTime(),
                EndDate = DateTime.UtcNow.AddDays(1)
            };
        }

        public DelegateCommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(AddTripAsync));

        public UserResponse User => JsonConvert.DeserializeObject<UserResponse>(Settings.User);

        public TripRequest Trip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        public async void AddTripAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }
            
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            
            if (!connection)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.InternetConnection, Languages.Accept);
                return;
            }

            Trip.UserEmail = User.Email;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.AddTrip(url, "/api", "/Trip/AddTrip", "bearer", token.Token, Trip);
            IsRunning = false;
            IsEnabled = true;
            List<TripResponse> trips = (List<TripResponse>)response.Result;
            Settings.Trips = JsonConvert.SerializeObject(trips);
            await _navigationService.NavigateAsync("/ExpensesMasterDetailPage/NavigationPage/TripsPage");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Trip.CityVisited))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorEmptyField,Languages.Accept);
                return false;
            }

            if (!_regexHelper.IsValidDate(Trip.StartDate.ToString()))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorDate, Languages.Accept);
                return false;
            }

            if (!_regexHelper.IsValidDate(Trip.EndDate.ToString()))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorDate, Languages.Accept);
                return false;
            }
            return true;
        }

    }
}
