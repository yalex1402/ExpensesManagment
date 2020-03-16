using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ExpensesManagment.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        public IEnumerable<SelectListItem> GetComboRoles()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "[Select a role...]" },
                new SelectListItem { Value = "1", Text = "Manager" },
                new SelectListItem { Value = "2", Text = "Employee" }
            };

            return list;
        }

        public IEnumerable<SelectListItem> GetComboExpenses()
        {
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "[Select a new expense...]"},
                new SelectListItem { Value = "1", Text = "Food"},
                new SelectListItem { Value = "2", Text = "Lodging"},
                new SelectListItem { Value = "3", Text = "Transport"},
                new SelectListItem { Value = "4", Text = "Oil"},
                new SelectListItem { Value = "5", Text = "Healthcare"},
                new SelectListItem { Value = "6", Text = "Phone"},
                new SelectListItem { Value = "7", Text = "Other"}
            };

            return list;
        }
    }
}
