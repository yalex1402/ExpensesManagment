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
    public class TripDetailPageViewModel : ViewModelBase
    {
        private TripResponse _trip;

        public TripDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public TripResponse Trip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("trip"))
            {
                _trip = parameters.GetValue<TripResponse>("trip");
                Title = $"{_trip.CityVisited} expenses";
            }
        }

    }
}
