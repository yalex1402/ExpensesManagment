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
    }

}
