using ExpensesManagment.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public interface IExpenseHelper
    {
        Task<ExpenseTypeEntity> GetExpenseTypeAsync(string name);

        Task<ExpenseTypeEntity> GetExpenseTypeAsync(int id);

        Task<List<ExpenseEntity>> GetExpesesAsync(int tripId);

    }
}
