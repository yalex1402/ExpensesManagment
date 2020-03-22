using ExpensesManagment.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpensesManagment.Web.Models
{
    public class ExpenseViewModel : ExpenseEntity
    {

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Register expense as")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a role.")]
        public int ExpenseId { get; set; }

        public IEnumerable<SelectListItem> ExpensesType { get; set; }

        [Display(Name = "Picture File")]
        public IFormFile PictureFile { get; set; }

        [Display(Name = "Logo")]
        public string LogoPath { get; set; }
    }
}
