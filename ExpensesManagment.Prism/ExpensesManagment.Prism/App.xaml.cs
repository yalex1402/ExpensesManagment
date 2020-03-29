using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Common.Services;
using ExpensesManagment.Prism.ViewModels;
using ExpensesManagment.Prism.Views;
using Newtonsoft.Json;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Syncfusion.Licensing;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ExpensesManagment.Prism
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MjE2NDM0QDMxMzcyZTM0MmUzMGhXSFduR2V3NzNDY2xGUTVRUk1YNFJjajZTQmV4c3A2ZmlvRjBjYTEwb0E9;MjE2NDM1QDMxMzcyZTM0MmUzMGVpZWhNS1FFQ2c4M1lkaFJFTEhuNjZXbThTZnMrbTNPSUJkRVUwRUlnaFk9");
            InitializeComponent();
            if (Settings.IsLogin)
            {
                List<TripResponse> trips = JsonConvert.DeserializeObject<List<TripResponse>>(Settings.Trips);
                NavigationParameters parameters = new NavigationParameters
                {
                    {"trips" , trips}
                };

                await NavigationService.NavigateAsync("/ExpensesMasterDetailPage/NavigationPage/TripsPage", parameters);
                return;
            }
            await NavigationService.NavigateAsync("ExpensesMasterDetailPage/NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<TripsPage, TripsPageViewModel>();
            containerRegistry.RegisterForNavigation<ExpensesMasterDetailPage, ExpensesMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
        }
    }
}
