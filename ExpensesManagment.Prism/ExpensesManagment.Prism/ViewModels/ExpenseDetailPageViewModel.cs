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
    public class ExpenseDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ExpenseResponse _expense;

        public ExpenseDetailPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Expense Detail";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("expense"))
            {
                _expense = parameters.GetValue<ExpenseResponse>("expense");
                Title = $"{_expense.Type.Name} detail";
            }
        }
    }
}
