﻿using ExpensesManagment.Common.Helpers;
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
                await NavigationService.NavigateAsync("/ExpensesMasterDetailPage/NavigationPage/TripsPage");
                return;
            }
            await NavigationService.NavigateAsync("ExpensesMasterDetailPage/NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<TripsPage, TripsPageViewModel>();
            containerRegistry.RegisterForNavigation<ExpensesMasterDetailPage, ExpensesMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<AddTripPage, AddTripPageViewModel>();
            containerRegistry.RegisterForNavigation<TripDetailPage, TripDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ExpenseDetailPage, ExpenseDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<RememberPasswordPage, RememberPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyTripPage, ModifyTripPageViewModel>();
            containerRegistry.RegisterForNavigation<DeleteTripPage, DeleteTripPageViewModel>();
            containerRegistry.RegisterForNavigation<AddExpensePage, AddExpensePageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyExpensePage, ModifyExpensePageViewModel>();
            containerRegistry.RegisterForNavigation<DeleteExpensePage, DeleteExpensePageViewModel>();
        }
    }
}
