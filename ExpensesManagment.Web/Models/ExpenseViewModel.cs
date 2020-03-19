using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpensesManagment.Web.Models
{
    public class ExpenseViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Register expense as")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a role.")]
        public int ExpenseId { get; set; }

        public IEnumerable<SelectListItem> ExpensesType { get; set; }

        [Display(Name = "Addicional Notes")]
        [MaxLength(280, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Details { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public float Value { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateToLocal => Date.ToLocalTime();

        [Display(Name = "Picture File")]
        public IFormFile PictureFile { get; set; }

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }

        [Display(Name = "Logo")]
        public string LogoPath { get; set; }
    }
}
