using System.ComponentModel.DataAnnotations;

namespace ExpensesManagment.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
