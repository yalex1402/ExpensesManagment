using ExpensesManagment.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Models
{
    public class TripViewModel 
    {
        public string UserId { get; set; }

        public int TripId { get; set; }

        [Display(Name ="Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDateLocal => StartDate.ToLocalTime();

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDateLocal => EndDate.ToLocalTime();

        [Display(Name = "City Visited")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(120, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string CityVisited { get; set; }

        public UserEntity User { get; set; }
    }
}
