using ExpensesManagment.Common.Enums;
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
            UserEntity user2 = await CheckUserAsync("1234", "Facundo", "Fortunatti", "fortunattibernard@gmail.com", "304 329 35 82", UserType.Employee);
            UserEntity user3 = await CheckUserAsync("1234", "Carolina", "Muñoz", "caroml98@hotmail.com", "304 329 35 82", UserType.Employee);
            await CheckExpenseTypesAsync();
            await CheckTripsAsync(user, user2, user3);
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
                _dataContext.Add(new ExpenseTypeEntity { Name = "Other" });
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
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Manager.ToString());
            await _userHelper.CheckRoleAsync(UserType.Employee.ToString());
        }

        private async Task CheckTripsAsync(UserEntity employee, UserEntity employee2, UserEntity employee3)
        {
            if (!_dataContext.Trips.Any())
            {
                _dataContext.Trips.Add(new TripEntity
                {
                    User = employee,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddHours(27),
                    CityVisited = "New York",
                    Expenses = new List<ExpenseEntity>
                    {
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Breakfast in a restaurant.",
                            Value = 25.31f,
                            Date = DateTime.UtcNow,
                            PicturePath = $"~/images/Expenses/breakfast.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        },
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Hotel",
                            Value = 449.13f,
                            Date = DateTime.UtcNow.AddMinutes(35),
                            PicturePath = $"~/images/Expenses/hotel_bill.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Lodging")
                        },
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Something to eat at noon",
                            Value = 36.68f,
                            Date = DateTime.UtcNow.AddMinutes(90),
                            PicturePath = $"~/images/Expenses/bill_noon.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        }
                    }
                });
                _dataContext.Trips.Add(new TripEntity
                {
                    User = employee,
                    StartDate = DateTime.UtcNow.AddHours(3),
                    EndDate = DateTime.UtcNow.AddHours(32),
                    CityVisited = "Chicago",
                    Expenses = new List<ExpenseEntity>
                    {
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Breakfast in a restaurant.",
                            Value = 28f,
                            Date = DateTime.UtcNow.AddHours(4),
                            PicturePath = $"~/images/Expenses/breakfast.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        },
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Hotel",
                            Value = 449.13f,
                            Date = DateTime.UtcNow.AddHours(5),
                            PicturePath = $"~/images/Expenses/hotel_bill.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Lodging")
                        },
                        new ExpenseEntity
                        {
                            User = employee,
                            Details = "Something to eat at noon",
                            Value = 36.68f,
                            Date = DateTime.UtcNow.AddHours(5.5),
                            PicturePath = $"~/images/Expenses/bill_noon.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        }
                    }
                });
                _dataContext.Trips.Add(new TripEntity
                {
                    User = employee2,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddHours(27),
                    CityVisited = "Philadelphia",
                    Expenses = new List<ExpenseEntity>
                    {
                        new ExpenseEntity
                        {
                            User = employee2,
                            Details = "Breakfast in a restaurant.",
                            Value = 25.31f,
                            Date = DateTime.UtcNow,
                            PicturePath = $"~/images/Expenses/breakfast.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        },
                        new ExpenseEntity
                        {
                            User = employee2,
                            Details = "Hotel",
                            Value = 520.3f,
                            Date = DateTime.UtcNow.AddMinutes(35),
                            PicturePath = $"~/images/Expenses/hotel_bill.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Lodging")
                        },
                        new ExpenseEntity
                        {
                            User = employee2,
                            Details = "Something to eat at noon",
                            Value = 73.5f,
                            Date = DateTime.UtcNow.AddMinutes(90),
                            PicturePath = $"~/images/Expenses/bill_noon.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        }
                    }
                });
                _dataContext.Trips.Add(new TripEntity
                {
                    User = employee3,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddHours(27),
                    CityVisited = "Florida",
                    Expenses = new List<ExpenseEntity>
                    {
                        new ExpenseEntity
                        {
                            User = employee3,
                            Details = "Breakfast in a restaurant.",
                            Value = 25.31f,
                            Date = DateTime.UtcNow,
                            PicturePath = $"~/images/Expenses/breakfast.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Food")
                        },
                        new ExpenseEntity
                        {
                            User = employee3,
                            Details = "Hotel",
                            Value = 449.13f,
                            Date = DateTime.UtcNow.AddMinutes(35),
                            PicturePath = $"~/images/Expenses/hotel_bill.jpg",
                            ExpenseType = await _expenseHelper.GetExpenseTypeAsync("Lodging")
                        },
                        new ExpenseEntity
                        {
                            User = employee3,
                            Details = "Something to eat at noon",
                            Value = 36.68f,
                            Date = DateTime.UtcNow.AddMinutes(90),
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
