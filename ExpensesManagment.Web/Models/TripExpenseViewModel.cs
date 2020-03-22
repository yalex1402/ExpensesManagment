using ExpensesManagment.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Models
{
    public class TripExpenseViewModel 
    {
        public string UserId { get; set; }

        public int TripId { get; set; }

        public ICollection<ExpenseEntity> Expenses { get; set; }

        public TripEntity Trip { get; set; }

        public UserEntity User { get; set; }

    }
}
