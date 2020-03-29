using ExpensesManagment.Common.Models;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<ExpenseEntity> ToAddExpenseEntity(ExpenseViewModel model, string picturePath);

        Task<ExpenseEntity> ToEditExpenseEntity(ExpenseViewModel model, string picturePath);

        Task<UserTripDetailViewModel> ToUserTripDetailViewModel(string id);

        Task<TripExpenseViewModel> ToTripExpenseViewModel(int? id);

        Task<TripEntity> ToAddTripEntity(TripViewModel model);

        Task<TripEntity> ToEditTripEntity(TripViewModel model);

        Task<TripViewModel> ToTripViewModel(TripEntity model);

        TripResponse ToTripResponse(TripEntity tripEntity);

        List<TripResponse> ToTripResponse(List<TripEntity> trips);

        UserResponse ToUserResponse(UserEntity userEntity);

        Task<TripEntity> ToTripEntity(TripRequest tripRequest);

        Task<ExpenseEntity> ToExpenseEntity(ExpenseRequest expenseRequest);
    }
}
