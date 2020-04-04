using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
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
    public class DeleteExpensePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ExpenseResponse _expense;
        private TripResponse _trip;
        private UserResponse _user;
        private DelegateCommand _deleteTripCommand;
        private string _email;
        private bool _isRunning;
        private bool _isEnabled;

        public DeleteExpensePageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Delete Expense";
            _trip = JsonConvert.DeserializeObject<TripResponse>(Settings.TripSelected);
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            IsRunning = false;
            IsEnabled = true;
        }

        public DelegateCommand DeleteTripCommand => _deleteTripCommand ?? (_deleteTripCommand = new DelegateCommand(DeleteAsync));

        public ExpenseResponse Expense
        {
            get => _expense;
            set => SetProperty(ref _expense, value);
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
                IsRunning = false;
                IsEnabled = true;
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.DeleteExpenseAsync(url, "/api", $"/Trip/DeleteExpense/{Expense.Id}", "bearer", token.Token);

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
            Settings.TripSelected = null;
            await App.Current.MainPage.DisplayAlert("Ok", "Expense was deleted successfully", "Accept");
            await _navigationService.NavigateAsync("/ExpensesMasterDetailPage/NavigationPage/TripsPage");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must write a email", "Accept");
                return false;
            }

            if (Email != _user.Email)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email doesn't exist", "Accept");
                return false;
            }

            return true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("expense"))
            {
                Expense = parameters.GetValue<ExpenseResponse>("expense");
            }
        }
    }
}
