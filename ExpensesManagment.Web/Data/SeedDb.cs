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
                            PicturePath = $"~/images/Expenses/breakfast.jpg"
                        } 
                    }
                });

                await _dataContext.SaveChangesAsync();
            }

        }
    }
}
