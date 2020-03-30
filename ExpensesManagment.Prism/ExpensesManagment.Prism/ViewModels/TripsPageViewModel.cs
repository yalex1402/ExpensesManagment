using ExpensesManagment.Common.Helpers;
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
    public class TripsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private List<TripItemViewModel> _trips;
        private DelegateCommand _addCommand;
        private bool _isRunning;

        public TripsPageViewModel(INavigationService navigationService,
            IApiService apiService) :base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Trips";
            _isRunning = false;
            LoadMyTrips();
        }

        public DelegateCommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(AddTripAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public List<TripItemViewModel> Trips
        {
            get => _trips;
            set => SetProperty(ref _trips, value);
        }

        public async void AddTripAsync()
        {
            await _navigationService.NavigateAsync(nameof(AddTripPage));
        }

        private async void LoadMyTrips()
        {
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection", "Accept");
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            List<TripItemViewModel> trips = JsonConvert.DeserializeObject<List<TripItemViewModel>>(Settings.Trips);
            IsRunning = false;
            Trips = trips.Select(t => new TripItemViewModel(_navigationService)
            {
                CityVisited = t.CityVisited,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Id = t.Id,
                Expenses = t.Expenses,
                User = t.User
            }).ToList();
        }

    }
}
