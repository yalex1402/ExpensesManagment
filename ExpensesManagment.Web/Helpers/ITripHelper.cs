using ExpensesManagment.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public interface ITripHelper
    {
        Task<List<TripEntity>> GetTrips(string id);

        Task<TripEntity> GetTripAsync(int? id);

        Task<ExpenseEntity> GetExpenseAsync(int? id);
    }
}
