using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public class TripHelper: ITripHelper
    {
        private readonly DataContext _dataContext;

        public TripHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<TripEntity>> GetTrips(string id)
        {
            List<TripEntity> tripEntity = await _dataContext.Trips
                .Include(t => t.Expenses)
                .Include(t => t.User)
                .Where(t => t.User.Id == id)
                .OrderBy(t => t.StartDate)
                .ToListAsync();
            return tripEntity;
        }

        public async Task<TripEntity> GetTripAsync(int? id)
        {
            TripEntity tripEntity = await _dataContext.Trips
                .Include(t => t.User)
                .Include(t => t.Expenses)
                .ThenInclude (e => e.ExpenseType)
                .FirstOrDefaultAsync(t => t.Id == id);
            return tripEntity;
        }

        public async Task<ExpenseEntity> GetExpenseAsync(int? id)
        {
            ExpenseEntity expenseEntity = await _dataContext.Expenses
                .Include(e => e.ExpenseType)
                .Include(e => e.Trip)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            return expenseEntity;
        }
    }
}
