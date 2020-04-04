using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesManagment.Prism.ViewModels
{
    public class DeleteExpensePageViewModel : ViewModelBase
    {
        public DeleteExpensePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Delete Expense";
        }
    }
}
