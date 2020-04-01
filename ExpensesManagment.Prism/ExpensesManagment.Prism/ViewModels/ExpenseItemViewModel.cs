using ExpensesManagment.Common.Models;
using ExpensesManagment.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesManagment.Prism.ViewModels
{
    public class ExpenseItemViewModel : ExpenseResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectExpenseCommand;

        public ExpenseItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectExpenseCommand => _selectExpenseCommand ?? (_selectExpenseCommand = new DelegateCommand(SelectExpenseAsync));

        private async void SelectExpenseAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "expense", this }
            };

            await _navigationService.NavigateAsync(nameof(ExpenseDetailPage), parameters);
        }
    }
}
