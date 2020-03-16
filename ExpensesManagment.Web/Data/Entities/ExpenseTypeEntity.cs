using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Data.Entities
{
    public class ExpenseTypeEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LogoPath { get; set; }

        public ICollection<ExpenseEntity> Expenses { get; set; }

    }
}
