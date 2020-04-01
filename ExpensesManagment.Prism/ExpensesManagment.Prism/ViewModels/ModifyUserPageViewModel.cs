using ExpensesManagment.Common.Enums;
using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
using ExpensesManagment.Prism.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpensesManagment.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IFilesHelper _filesHelper;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private ImageSource _image;
        private UserResponse _user;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;

        public ModifyUserPageViewModel(INavigationService navigationService, 
            IFilesHelper filesHelper, 
            IApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _filesHelper = filesHelper;
            _apiService = apiService;
            Title = "Modify User";
            IsEnabled = true;
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            Image = User.PictureFullPath;
        }

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new DelegateCommand(ChangePasswordAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
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
            var isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }
            IsRunning = true;
            IsEnabled = false;

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }

            var userRequest = new UserRequest
            {
                Document = User.Document,
                Email = User.Email,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Password = "123456", // It doesn't matter what is sent here. It is only for the model to be valid
                Phone = User.PhoneNumber,
                PictureArray = imageArray,
                UserTypeId = User.UserType == UserType.Employee ? 1 : 2,
                CultureInfo = "es"
            };

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.PutAsync(url, "/api", "/Account", userRequest, "bearer", token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Settings.User = JsonConvert.SerializeObject(User);
            ExpensesMasterDetailPageViewModel.GetInstance().ReloadUser();
            await App.Current.MainPage.DisplayAlert("Ok", "User updated successfully", "Accept");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.Document))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must write a document", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must write your first name", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must write your last name", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.PhoneNumber))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must write a phone number", "Accept");
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

        private async void ChangePasswordAsync()
        {
            await _navigationService.NavigateAsync(nameof(ChangePasswordPage));
        }
    }

}
