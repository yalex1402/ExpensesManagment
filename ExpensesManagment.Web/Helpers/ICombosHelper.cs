using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ExpensesManagment.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboRoles();

        IEnumerable<SelectListItem> GetComboExpenses();
    }
}
