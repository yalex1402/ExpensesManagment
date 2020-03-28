using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace ExpensesManagment.Prism.ViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService,
            IApiService apiService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            if (PageName == "LoginPage" && Settings.IsLogin)
            {
                Settings.IsLogin = false;
                Settings.User = null;
                Settings.Token = null;
            }

            if (PageName == "TripsPage" && !Settings.IsLogin)
            {
                await _navigationService.NavigateAsync($"/ExpensesMasterDetailPage/NavigationPage/LoginPage");
                return;
            }

            if (PageName == "TripsPage" && Settings.IsLogin)
            {
                UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                MyTripsRequest myTripsRequest = new MyTripsRequest
                {
                    UserId = userResponse.Id,
                    StartDate = DateTime.Parse("2020-01-01"),
                    EndDate = DateTime.UtcNow
                };
                string url = App.Current.Resources["UrlAPI"].ToString();
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                Response response = await _apiService.GetTripsByUser(url, "api", "/Trip/GetTrips", "bearer", token.Token, myTripsRequest);
                List<TripResponse> trips = (List<TripResponse>)response.Result;
                NavigationParameters parameters = new NavigationParameters
                {
                    {"trips" , trips}
                };

                await _navigationService.NavigateAsync("/ExpensesMasterDetailPage/NavigationPage/TripsPage", parameters);
                return;
            }

            await _navigationService.NavigateAsync($"/ExpensesMasterDetailPage/NavigationPage/{PageName}");
        }
    }

}
