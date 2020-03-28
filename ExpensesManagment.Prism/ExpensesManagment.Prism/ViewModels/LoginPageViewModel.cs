﻿using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
using ExpensesManagment.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesManagment.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private string _password;
        private List<TripResponse> _trips;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;

        public LoginPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

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

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public List<TripResponse> Trips
        {
            get => _trips;
            set => SetProperty(ref _trips, value);
        }

        private async void LoginAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Email couldn't be empty",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Password couldn't be empty",
                    "Accept");
                return;
            }
            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet conection", "Accept");
                return;
            }

            TokenRequest request = new TokenRequest
            {
                Password = Password,
                UserName = Email
            };

            Response response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Email or password are incorrect", "Accept");
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            EmailRequest emailRequest = new EmailRequest
            {
                CultureInfo = "es",
                Email = Email
            };

            Response response2 = await _apiService.GetUserByEmail(url, "api", "/Account/GetUser", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response2.Result;

            Settings.User = JsonConvert.SerializeObject(userResponse);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            MyTripsRequest myTripsRequest = new MyTripsRequest
            {
                UserId = userResponse.Id,
                StartDate = DateTime.Parse("2020-01-01"),
                EndDate = DateTime.UtcNow
            };

            Response response3 = await _apiService.GetTripsByUser(url, "api", "/Trip/GetTrips", "bearer", token.Token, myTripsRequest);
            Trips = (List<TripResponse>)response3.Result;

            NavigationParameters parameters = new NavigationParameters
            {
                {"trips" , Trips}
            };
            
            await _navigationService.NavigateAsync("NavigationPage/TripsPage",parameters);
            Password = string.Empty;

        }

        private async void RegisterAsync()
        {
            await _navigationService.NavigateAsync(nameof(RegisterPage));
        }
    }
}