using ExpensesManagment.Common.Interfaces;
using ExpensesManagment.Prism.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ExpensesManagment.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }

        public static string Trips => Resource.Trips;

        public static string Error => Resource.Error;

        public static string Accept => Resource.Accept;

        public static string InternetConnection => Resource.InternetConnection;

        public static string Expenses => Resource.Expenses;

        public static string Ok => Resource.Ok;

        public static string ErrorEmail => Resource.ErrorEmail;

        public static string RecoverPassword => Resource.RecoverPassword;

        public static string Register => Resource.Register;

        public static string ErrorEmptyField => Resource.ErrorEmptyField;

        public static string ErrorRole => Resource.ErrorRole;

        public static string ErrorPassword => Resource.ErrorPassword;

        public static string ErrorPassword2 => Resource.ErrorPassword2;

        public static string FromGallery => Resource.FromGallery;

        public static string FromCamera => Resource.FromCamera;

        public static string Cancel => Resource.Cancel;

        public static string PictureSource => Resource.PictureSource;

        public static string ModifyUser => Resource.ModifyUser;

        public static string UserUpdated => Resource.UserUpdated;

        public static string ModifyTrip => Resource.ModifyTrip;

        public static string TripUpdated => Resource.TripUpdated;

        public static string ModifyExpense => Resource.ModifyExpense;

        public static string ExpenseUpdated => Resource.ExpenseUpdated;

        public static string ErrorDate => Resource.ErrorDate;

        public static string ErrorValue => Resource.ErrorValue;

        public static string Login => Resource.Login;

        public static string Logout => Resource.Logout;

        public static string ErrorLogin => Resource.ErrorLogin;

        public static string MyTrips => Resource.MyTrips;

        public static string DeleteTrip => Resource.DeleteTrip;

        public static string DeleteExpense => Resource.DeleteExpense;

        public static string AddTrip => Resource.AddTrip;

        public static string AddExpense => Resource.AddExpense;

        public static string TripDeleted => Resource.TripDeleted;

        public static string ExpenseDeleted => Resource.ExpenseDeleted;

        public static string ChangePassword => Resource.ChangePassword;

        public static string ErrorPassword3 => Resource.ErrorPassword3;

        public static string ErrorExpenseType => Resource.ErrorExpenseType;
    }
}
