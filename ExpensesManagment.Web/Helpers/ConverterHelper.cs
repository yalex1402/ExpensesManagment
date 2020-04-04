
using ExpensesManagment.Common.Helpers;
using ExpensesManagment.Common.Models;
using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            TripEntity tripEntity = await _tripHelper.GetTripAsync(model.TripId);
            return new ExpenseEntity
            {
                Trip = tripEntity,
                User = tripEntity.User,
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(model.ExpenseId),
                Details = model.Details,
                Value = model.Value,
                Date = model.Date.ToUniversalTime(),
                PicturePath = picturePath,
            };
        }

        public async Task<ExpenseEntity> ToEditExpenseEntity(ExpenseViewModel model, string picturePath)
        {
            TripEntity tripEntity = await _tripHelper.GetTripAsync(model.TripId);
            return new ExpenseEntity
            {
                Id = model.Id,
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(model.ExpenseId),
                Details = model.Details,
                Value = model.Value,
                Date = model.Date.ToUniversalTime(),
                PicturePath = picturePath,
                Trip = tripEntity,
                User = tripEntity.User,
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

        public async Task<TripExpenseViewModel> ToTripExpenseViewModel(int? id)
        {
            List<ExpenseEntity> expenseEntities = await _expenseHelper.GetExpesesAsync(id);
            TripEntity tripEntity = await _tripHelper.GetTripAsync(id);

            return new TripExpenseViewModel
            {
                Trip = tripEntity,
                TripId = tripEntity.Id,
                Expenses = expenseEntities,
                User = tripEntity.User
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

        public List<TripResponse> ToTripResponse(List<TripEntity> trips)
        {
            List<TripResponse> list = new List<TripResponse>();
            
            foreach(TripEntity tripEntity in trips)
            {
                list.Add(ToTripResponse(tripEntity));
            }

            return list;
        }

        public TripResponse ToTripResponse (TripEntity tripEntity)
        {
            return new TripResponse
            {
                Id = tripEntity.Id,
                CityVisited = tripEntity.CityVisited,
                StartDate = tripEntity.StartDate,
                EndDate = tripEntity.EndDate,
                User = ToUserResponse(tripEntity.User),
                Expenses = tripEntity.Expenses?.Select(ex => new ExpenseResponse
                {
                    Id = ex.Id,
                    Details = ex.Details,
                    Date = ex.Date,
                    Value = ex.Value,
                    PicturePath = ex.PicturePath,
                    Type = ToExpenseTypeResponse(ex.ExpenseType)
                }).ToList()
            };
        }

        public UserResponse ToUserResponse (UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return null;
            }
            return new UserResponse
            {
                Id = userEntity.Id,
                Document = userEntity.Document,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                PhoneNumber = userEntity.PhoneNumber,
                PicturePath = userEntity.PicturePath,
                UserType = userEntity.UserType
            };
        }

        public ExpenseTypeResponse ToExpenseTypeResponse (ExpenseTypeEntity type)
        {
            if(type == null)
            {
                return null;
            }
            return new ExpenseTypeResponse
            {
                Id = type.Id,
                Name = type.Name,
                LogoPath = type.LogoPath
            };
        }

        public async Task<TripEntity> ToTripEntity (TripRequest tripRequest)
        {
            UserEntity userEntity = await _userHelper.GetUserAsync(tripRequest.UserEmail);
            return new TripEntity
            {
                CityVisited = tripRequest.CityVisited,
                StartDate = tripRequest.StartDate,
                EndDate = tripRequest.EndDate,
                User = userEntity
            };
        }

        public async Task<ExpenseEntity> ToExpenseEntity (ExpenseRequest expenseRequest)
        {
            TripEntity tripEntity = await _tripHelper.GetTripAsync(expenseRequest.TripId);
            return new ExpenseEntity
            {
                Details = expenseRequest.Details,
                Date = expenseRequest.Date,
                Trip = tripEntity,
                User = await _userHelper.GetUserAsync(tripEntity.User.Email),
                Value = expenseRequest.Value,
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(expenseRequest.ExpenseTypeId)
        };
        }

        public ExpenseResponse ToExpenseResponse (ExpenseEntity expenseEntity)
        {
            return new ExpenseResponse
            {
                Id = expenseEntity.Id,
                Details = expenseEntity.Details,
                Date = expenseEntity.Date,
                PicturePath = expenseEntity.PicturePath,
                Value = expenseEntity.Value,
                Type = ToExpenseTypeResponse(expenseEntity.ExpenseType)
            };
        }
    }
}
