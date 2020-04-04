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
using Xamarin.Forms;

namespace ExpensesManagment.Prism.ViewModels
{
    public class ExpenseDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ExpenseResponse _expense;
        private ImageSource _image;
        private DelegateCommand _editExpenseCommand;
        private DelegateCommand _deleteExpenseCommand;
        private bool _isRunning;
        private bool _isEnabled;

        public ExpenseDetailPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            IsRunning = false;
            IsEnabled = true;
        }

        public DelegateCommand EditExpenseCommand => _editExpenseCommand ?? (_editExpenseCommand = new DelegateCommand(EditExpenseAsync));

        public DelegateCommand DeleteExpenseCommand => _deleteExpenseCommand ?? (_deleteExpenseCommand = new DelegateCommand(DeleteExpenseAsync));

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

        private async void EditExpenseAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                {"expense", Expense }
            };
            await _navigationService.NavigateAsync(nameof(ModifyExpensePage),parameters);
        }

        private async void DeleteExpenseAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                {"expense", Expense }
            };
            await _navigationService.NavigateAsync(nameof(DeleteExpensePage), parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("expense"))
            {
                Expense = parameters.GetValue<ExpenseResponse>("expense");
                Image = Expense.PictureFullPath;
                Title = $"{Expense.Type.Name} detail";
            }
        }
    }
}
