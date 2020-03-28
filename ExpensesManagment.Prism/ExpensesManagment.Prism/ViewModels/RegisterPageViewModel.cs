using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpensesManagment.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        public RegisterPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Register";
        }
    }
}
