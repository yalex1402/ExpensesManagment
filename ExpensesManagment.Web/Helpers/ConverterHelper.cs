
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
        private readonly IUserHelper _userHelper;

        public ConverterHelper(DataContext dataContext,
            IExpenseHelper expenseHelper,
            ITripHelper tripHelper,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _expenseHelper = expenseHelper;
            _tripHelper = tripHelper;
            _userHelper = userHelper;
        }
        public async Task<ExpenseEntity> ToAddExpenseEntity(ExpenseViewModel model, string picturePath)
        {
            return new ExpenseEntity
            {
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(model.ExpenseId),
                Details = model.Details,
                Value = model.Value,
                Date = model.Date.ToUniversalTime(),
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
                Date = model.Date.ToUniversalTime(),
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

        public async Task<TripEntity> ToAddTripEntity (TripViewModel model)
        {
            return new TripEntity
            {
                CityVisited = model.CityVisited,
                StartDate = model.StartDate.ToUniversalTime(),
                EndDate = model.EndDate.ToUniversalTime(),
                User = await _dataContext.Users.FindAsync(model.UserId)
            };
        }

        public async Task<TripEntity> ToEditTripEntity(TripViewModel model)
        {
            return new TripEntity
            {
                Id = model.TripId,
                CityVisited = model.CityVisited,
                StartDate = model.StartDate.ToUniversalTime(),
                EndDate = model.EndDate.ToUniversalTime(),
                User = await _dataContext.Users.FindAsync(model.UserId),
                Expenses = await _expenseHelper.GetExpesesAsync(model.TripId)
            };
        }

        public async Task<TripViewModel> ToTripViewModel (TripEntity model)
        {
            UserEntity userEntity = await _userHelper.GetUserAsync(model.User.Email);
            return new TripViewModel
            {
                TripId = model.Id,
                StartDate = model.StartDate.ToLocalTime(),
                EndDate = model.EndDate.ToLocalTime(),
                CityVisited = model.CityVisited,
                User = userEntity,
                UserId = userEntity.Id
            };
        }
    }
}
