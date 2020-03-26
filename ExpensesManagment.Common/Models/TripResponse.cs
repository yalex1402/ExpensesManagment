using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesManagment.Common.Models
{
    public class TripResponse
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        public DateTime EndDate { get; set; }

        public DateTime EndDateLocal => EndDate.ToLocalTime();

        public string CityVisited { get; set; }

        public ICollection<ExpenseResponse> Expenses { get; set; }

        public UserResponse User { get; set; }
    }
}
