using ExpensesManagment.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Data.Entities
{
    public class ExpenseEntity
    {
        public int Id { get; set; }

        [Display(Name ="Addicional Notes")]
        [MaxLength(280, ErrorMessage = "The {0} field can not have more than {1} characters.") ]
        public string Details { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}",ApplyFormatInEditMode =true)]
        [Display(Name ="Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public float Value { get; set; }

        [Display(Name = "Picture Path")]
        public string PicturePath { get; set; }

        [Display(Name = "Expense Type")]
        public ExpenseType ExpenseName { get; set; }

    }
}
