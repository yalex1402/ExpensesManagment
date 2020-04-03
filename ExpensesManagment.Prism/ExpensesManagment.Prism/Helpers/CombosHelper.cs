using ExpensesManagment.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesManagment.Prism.Helpers
{
    public static class CombosHelper
    {
        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = "Employee" },
                new Role { Id = 2, Name = "Manager" }
            };
        }

        public static List<ExpenseType> GetExpenseTypes()
        {
            return new List<ExpenseType>
            {
                new ExpenseType { Id = 1, Name = "Food" },
                new ExpenseType { Id = 2, Name = "Lodging" },
                new ExpenseType { Id = 3, Name = "Transport" },
                new ExpenseType { Id = 4, Name = "Oil" },
                new ExpenseType { Id = 5, Name = "Healthcare" },
                new ExpenseType { Id = 6, Name = "Phone" },
                new ExpenseType { Id = 7, Name = "Other" },
            };
        }
    }

}
