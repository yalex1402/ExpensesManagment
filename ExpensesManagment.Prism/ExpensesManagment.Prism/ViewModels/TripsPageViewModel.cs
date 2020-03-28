using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
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
        private List<TripResponse> _trips;
        private bool _isRunning;

        public TripsPageViewModel(INavigationService navigationService,
            IApiService apiService) :base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Trips";
            _isRunning = false;
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public List<TripResponse> Trips
        {
            get => _trips;
            set => SetProperty(ref _trips, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("trips"))
            {
                Trips = parameters.GetValue<List<TripResponse>>("trips");
            }
        }

    }
}
