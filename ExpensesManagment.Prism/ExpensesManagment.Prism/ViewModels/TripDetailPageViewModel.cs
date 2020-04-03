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
    public class TripDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private TripResponse _trip;
        private List<ExpenseItemViewModel> _expenses;
        private bool _isRunning;
        private DelegateCommand _editTripCommand;
        private DelegateCommand _deleteTripCommand;
        private DelegateCommand _addExpenseCommand;

        public TripDetailPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Trip = JsonConvert.DeserializeObject<TripResponse>(Settings.TripSelected);
            _isRunning = false;
            LoadExpenses();
        }

        public DelegateCommand EditTripCommand => _editTripCommand ?? (_editTripCommand = new DelegateCommand(EditTripAsync));

        public DelegateCommand DeleteTripCommand => _deleteTripCommand ?? (_deleteTripCommand = new DelegateCommand(DeleteTripAsync));

        public DelegateCommand AddExpenseCommand => _addExpenseCommand ?? (_addExpenseCommand = new DelegateCommand(AddExpenseAsync));

        public TripResponse Trip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        public List<ExpenseItemViewModel> Expenses
        {
            get => _expenses;
            set => SetProperty(ref _expenses, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public async void LoadExpenses()
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
            IsRunning = false;
            Expenses = Trip.Expenses.Select(e => new ExpenseItemViewModel(_navigationService)
            {
                Id = e.Id,
                Details = e.Details,
                Date = e.Date,
                PicturePath = e.PicturePath,
                Type = e.Type,
                Value = e.Value
            }).ToList();
        }

        public async void EditTripAsync()
        {
            await _navigationService.NavigateAsync(nameof(ModifyTripPage));
        }

        public async void DeleteTripAsync()
        {
            await _navigationService.NavigateAsync(nameof(DeleteTripPage));
        }

        public async void AddExpenseAsync()
        {
            await _navigationService.NavigateAsync(nameof(AddExpensePage));
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
