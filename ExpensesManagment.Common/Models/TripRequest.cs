using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExpensesManagment.Common.Models
{
    public class TripRequest
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime StartDateLocal => StartDate.ToLocalTime();

        [Required]
        public DateTime EndDate { get; set; }

        public DateTime EndDateLocal => EndDate.ToLocalTime();

        [Required]
        public string CityVisited { get; set; }

        public ICollection<ExpenseResponse> Expenses { get; set; }
        
        [Required]
        public string UserEmail { get; set; }
    }
}
