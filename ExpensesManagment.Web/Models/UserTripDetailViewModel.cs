using ExpensesManagment.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Models
{
    public class UserTripDetailViewModel 
    {
        public string UserId { get; set; }

        [Display(Name ="Name")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Display(Name = "Photo")]
        public string UserPicture { get; set; }

        public ICollection<TripEntity> Trips { get; set; }

    }
}
