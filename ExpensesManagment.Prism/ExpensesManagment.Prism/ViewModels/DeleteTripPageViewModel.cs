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
    public class DeleteTripPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private TripResponse _trip;
        private DelegateCommand _deleteTripCommand;
        private UserResponse _user;
        private string _email;
        private bool _isRunning;
        private bool _isEnabled;

        public DeleteTripPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.DeleteTrip;
            _trip = JsonConvert.DeserializeObject<TripResponse>(Settings.TripSelected);
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            _isRunning = false;
            _isEnabled = true;
        }

        public DelegateCommand DeleteTripCommand => _deleteTripCommand ?? (_deleteTripCommand = new DelegateCommand(DeleteAsync));

        public TripResponse Trip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
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

        public async void DeleteAsync()
        {
            IsRunning = true;
            IsEnabled = false; 
            bool isValid = await ValidateDataAsync();

            if (!isValid)
            {
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.DeleteTripAsync(url, "/api", $"/Trip/{Trip.Id}","bearer",token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
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
            Settings.TripSelected = null;
            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.TripDeleted, Languages.Accept);
            await _navigationService.NavigateAsync("/ExpensesMasterDetailPage/NavigationPage/TripsPage");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorEmptyField, Languages.Accept);
                return false;
            }

            if (Email != _user.Email)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorEmail, Languages.Accept);
                return false;
            }

            return true;
        }

    }
}
