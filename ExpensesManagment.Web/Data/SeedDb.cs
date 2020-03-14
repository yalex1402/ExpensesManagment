using ExpensesManagment.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;

        public SeedDb(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckTripsAsync();
        }

        private async Task CheckTripsAsync()
        {
            if (!_dataContext.Trips.Any())
            {
                _dataContext.Trips.Add(new TripEntity
                {
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(45),
                    CityVisited = "New York",
                    Expenses = new List<ExpenseEntity> 
                    {
                        new ExpenseEntity
                        {
                            Details = "Breakfast in a restaurant.",
                            Value = 25.31f,
                            PicturePath = $"~/images/Expenses/breakfast.jpg",
                            ExpenseName = Common.ExpenseType.Food
                        },
                        new ExpenseEntity
                        {
                            Details = "Hotel",
                            Value = 449.13f,
                            PicturePath = $"~/images/Expenses/hotel_bill.jpg",
                            ExpenseName = Common.ExpenseType.Lodging
                        },
                        new ExpenseEntity
                        {
                            Details = "Something to eat at noon",
                            Value = 36.68f,
                            PicturePath = $"~/images/Expenses/bill_noon.jpg",
                            ExpenseName = Common.ExpenseType.Food
                        }
                    }
                });

                await _dataContext.SaveChangesAsync();
            }

        }
    }
}
