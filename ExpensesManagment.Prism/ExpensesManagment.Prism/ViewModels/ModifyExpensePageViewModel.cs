using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
using ExpensesManagment.Prism.Helpers;
using ExpensesManagment.Prism.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpensesManagment.Prism.ViewModels
{
    public class ModifyExpensePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private ExpenseResponse _expense;
        private readonly UserResponse _user;
        private readonly TripResponse _trip;
        private ImageSource _image;
        private MediaFile _file;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changeImageCommand;
        private bool _isRunning;
        private bool _isEnabled;

        public ModifyExpensePageViewModel(INavigationService navigationService,
            IApiService apiService,
            IFilesHelper filesHelper) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Title = Languages.ModifyExpense;
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            _trip = JsonConvert.DeserializeObject<TripResponse>(Settings.TripSelected);
            IsEnabled = true;
            IsRunning = false;
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public ExpenseResponse Expense
        {
            get => _expense;
            set => SetProperty(ref _expense, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
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

        public async void SaveAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
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
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.InternetConnection, Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }

            ExpenseRequest expenseRequest = new ExpenseRequest
            {
                Id = Expense.Id,
                Details = Expense.Details,
                Date = Expense.Date,
                Value = Expense.Value,
                ExpenseTypeId = 1, //It doesn't because it doesn't change
                PictureArray = imageArray,
                TripId = _trip.Id,
                UserEmail = _user.Email
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.PutAsync(url, "/api", "/Trip/ModifyExpense", expenseRequest, "bearer", token.Token);

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
            Response response3 = await _apiService.GetTripAsync(url, "/api", $"/Trip/{_trip.Id}", "bearer", token.Token);
            TripResponse trip = (TripResponse)response3.Result;
            Settings.TripSelected = JsonConvert.SerializeObject(trip);
            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.ExpenseUpdated, Languages.Accept);
            await _navigationService.NavigateAsync("/ExpensesMasterDetailPage/NavigationPage/TripsPage");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Expense.Value.ToString()) || Expense.Value <= 0)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorValue, Languages.Accept);
                return false;
            }

            return true;
        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                "Source",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "From Camera")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                Image = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("expense"))
            {
                Expense = parameters.GetValue<ExpenseResponse>("expense");
                Image = Expense.PictureFullPath;
            }
        }
    }
}
