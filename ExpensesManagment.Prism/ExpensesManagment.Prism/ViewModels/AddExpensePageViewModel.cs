﻿using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
using ExpensesManagment.Prism.Helpers;
using ExpensesManagment.Prism.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpensesManagment.Prism.ViewModels
{
    public class AddExpensePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private ImageSource _image;
        private MediaFile _file;
        private ExpenseType _expenseType;
        private ObservableCollection<ExpenseType> _expenseTypes;
        private UserResponse _user;
        private TripResponse _trip;
        private ExpenseRequest _expense;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _addCommand;
        private bool _isRunning;
        private bool _isEnabled;

        public AddExpensePageViewModel(INavigationService navigationService,
            IApiService apiService,
            IFilesHelper filesHelper) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _filesHelper = filesHelper;
            Title = "Add new expense";
            Image = App.Current.Resources["UrlNoImage"].ToString();
            IsRunning = false;
            IsEnabled = true;
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            _trip = JsonConvert.DeserializeObject<TripResponse>(Settings.TripSelected);
            ExpenseTypes = new ObservableCollection<ExpenseType>(CombosHelper.GetExpenseTypes());
            Expense = new ExpenseRequest
            {
                Date = DateTime.UtcNow.ToLocalTime()
            };
        }

        public DelegateCommand AddCommand => _addCommand ?? (_addCommand = new DelegateCommand(AddExpenseAsync));

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public ExpenseRequest Expense
        {
            get => _expense;
            set => SetProperty(ref _expense, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public ExpenseType ExpenseType
        {
            get => _expenseType;
            set => SetProperty(ref _expenseType, value);
        }

        public ObservableCollection<ExpenseType> ExpenseTypes
        {
            get => _expenseTypes;
            set => SetProperty(ref _expenseTypes, value);
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

        public async void AddExpenseAsync()
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
                await App.Current.MainPage.DisplayAlert("Error", "Check the internet connection", "Accept");
                return;
            }
            
            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }
            
            Expense.PictureArray = imageArray;
            Expense.ExpenseTypeId = ExpenseType.Id;
            Expense.TripId = _trip.Id;
            Expense.UserEmail = _user.Email;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Response response = await _apiService.AddExpense(url, "/api", "/Trip/AddExpense", "bearer", token.Token, Expense);
            
            IsRunning = false;
            IsEnabled = true;

            TripResponse trip = (TripResponse)response.Result;
            
            NavigationParameters parameters = new NavigationParameters
            {
                { "trip", trip }
            };

            Settings.TripSelected = JsonConvert.SerializeObject(trip);
            await _navigationService.NavigateAsync(nameof(TripDetailPage), parameters);
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Expense.Value.ToString()) || Expense.Value <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Value is incorrect", "Accept");
                return false;
            }

            if (ExpenseType == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must select an expense type", "Accept");
                return false;
            }

            return true;
        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                "Picture Source",
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
    }
}
