using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<ExpenseEntity> ToExpenseEntity(AddExpenseViewModel model, string picturePath);
    }
}
