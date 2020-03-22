
using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly IExpenseHelper _expenseHelper;
        private readonly ITripHelper _tripHelper;

        public ConverterHelper(DataContext dataContext,
            IExpenseHelper expenseHelper,
            ITripHelper tripHelper)
        {
            _dataContext = dataContext;
            _expenseHelper = expenseHelper;
            _tripHelper = tripHelper;
        }
        public async Task<ExpenseEntity> ToAddExpenseEntity(ExpenseViewModel model, string picturePath)
        {
            return new ExpenseEntity
            {
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(model.ExpenseId),
                Details = model.Details,
                Value = model.Value,
                Date = model.Date,
                PicturePath = picturePath,
            };
        }

        public async Task<ExpenseEntity> ToEditExpenseEntity(ExpenseViewModel model, string picturePath)
        {
            return new ExpenseEntity
            {
                Id = model.Id,
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(model.ExpenseId),
                Details = model.Details,
                Value = model.Value,
                Date = model.Date,
                PicturePath = picturePath,
            };
        }

        public async Task<UserTripDetailViewModel> ToUserTripDetailViewModel(string id)
        {
            UserEntity user = await _dataContext.Users.FindAsync(id);
            return new UserTripDetailViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserName = user.FullName,
                UserPicture = user.PicturePath,
                Trips = await _tripHelper.GetTrips(id)
            };
        }
    }
}
