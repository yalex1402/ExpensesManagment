using ExpensesManagment.Common;
using ExpensesManagment.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesManagment.Web.Data.Entities
{
    public class ExpenseEntity
    {
        public int Id { get; set; }

        [Display(Name = "Addicional Notes")]
        [MaxLength(280, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Details { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public float Value { get; set; }

        [Display(Name = "Photo")]
        public string PicturePath { get; set; }

        public TripEntity Trip { get; set; }

        public UserEntity User { get; set; }

        public ExpenseTypeEntity ExpenseType { get; set; }
    }
}
