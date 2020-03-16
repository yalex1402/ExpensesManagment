﻿using ExpensesManagment.Common.Enums;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ExpensesManagment.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IExpenseHelper _expenseHelper;

        public SeedDb(DataContext dataContext,
            IUserHelper userHelper,
            IExpenseHelper expenseHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _expenseHelper = expenseHelper;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            UserEntity admin = await CheckUserAsync("1234", "Yesid", "Garcia", "yesidgarcialopez@gmail.com", "304 329 35 82", UserType.Admin);
            UserEntity manager = await CheckUserAsync("1234", "Alexander", "Garcia", "yagarcia1402@gmail.com", "304 329 35 82", UserType.Manager);
            UserEntity user = await CheckUserAsync("1234", "Yesid", "Garcia", "yesidgarcia229967@correo.itm.edu.co", "304 329 35 82", UserType.Employee);
            await CheckExpenseTypesAsync();
            await CheckTripsAsync(user);
        }

        private async Task CheckExpenseTypesAsync()
        {
            if (!_dataContext.ExpenseTypes.Any())
            {
                _dataContext.Add(new ExpenseTypeEntity { Name = "Food", LogoPath = $"~/images/Expenses/Food.png" });
                _dataContext.Add(new ExpenseTypeEntity { Name = "Lodging", LogoPath = $"~/images/Expenses/Lodging.png" });
                _dataContext.Add(new ExpenseTypeEntity { Name = "Transport", LogoPath = $"~/images/Expenses/Transport.png" });
                _dataContext.Add(new ExpenseTypeEntity { Name = "Oil", LogoPath = $"~/images/Expenses/Oil.png" });
                _dataContext.Add(new ExpenseTypeEntity { Name = "Healthcare", LogoPath = $"~/images/Expenses/Healthcare.png" });
                _dataContext.Add(new ExpenseTypeEntity { Name = "Phone", LogoPath = $"~/images/Expenses/Phone.png" });
                _dataContext.Add(new ExpenseTypeEntity { Name = "Other"});
            }

            await _dataContext.SaveChangesAsync();
        }

        private async Task<UserEntity> CheckUserAsync(
           string document,
           string firstName,
           string lastName,
           string email,
           string phone,
           UserType userType)
        {
            UserEntity user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Manager.ToString());
            await _userHelper.CheckRoleAsync(UserType.Employee.ToString());
        }

        private async Task CheckTripsAsync(UserEntity employee)
        {
            if (!_dataContext.Trips.Any())
            {
                _dataContext.Trips.Add(new TripEntity
                {
                    User = employee,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(45),
                    CityVisited = "New York",
                    Expenses = new List<ExpenseEntity>
                    {
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Breakfast in a restaurant.",
                            Value = 25.31f,
                            PicturePath = $"~/images/Expenses/breakfast.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        },
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Hotel",
                            Value = 449.13f,
                            PicturePath = $"~/images/Expenses/hotel_bill.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Lodging")
                        },
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Something to eat at noon",
                            Value = 36.68f,
                            PicturePath = $"~/images/Expenses/bill_noon.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        }
                    }
                });

                await _dataContext.SaveChangesAsync();
            }
        }

    }
}
