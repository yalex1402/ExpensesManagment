using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesManagment.Prism.ViewModels
{
    public class ModifyTripPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private TripResponse _trip;
        private readonly UserResponse _user;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _saveCommand;

        public ModifyTripPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Modify Trip";
            _trip = JsonConvert.DeserializeObject<TripResponse>(Settings.TripSelected);
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            _isEnabled = true;
            _isRunning = false;
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public TripResponse Trip
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

        private async void SaveAsync()
        {
            bool isValid = await ValidateDataAsync();

            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            TripRequest tripRequest = new TripRequest
            {
                Id = Trip.Id,
                CityVisited = Trip.CityVisited,
                StartDate = Trip.StartDate,
                EndDate = Trip.EndDate,
                UserEmail = _user.Email
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.PutAsync(url, "/api", "/Trip", tripRequest, "bearer", token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            MyTripsRequest myTripsRequest = new MyTripsRequest
            {
                UserId = _user.Id,
                StartDate = DateTime.Parse("2020-01-01"),
                EndDate = DateTime.UtcNow
            };

            Response response2 = await _apiService.GetTripsByUser(url, "api", "/Trip/GetTrips", "bearer", token.Token, myTripsRequest);
            List<TripResponse> trips = (List<TripResponse>)response2.Result;

            Settings.Trips = JsonConvert.SerializeObject(trips);
            Settings.TripSelected = JsonConvert.SerializeObject(Trip);
            await App.Current.MainPage.DisplayAlert("Ok", "Trip was updated successfully", "Accept");
            await _navigationService.NavigateAsync("/ExpensesMasterDetailPage/NavigationPage/TripsPage");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Trip.CityVisited))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must write a document", "Accept");
                return false;
            }

            if (Trip.EndDate < Trip.StartDate)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Invalid End Date, please try again", "Accept");
                return false;
            }

            return true;
        }
    }
}
