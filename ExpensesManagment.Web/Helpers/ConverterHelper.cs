
using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly IExpenseHelper _expenseHelper;

        public ConverterHelper(DataContext dataContext,
            IExpenseHelper expenseHelper)
        {
            _dataContext = dataContext;
            _expenseHelper = expenseHelper;
        }
        public async Task<ExpenseEntity> ToExpenseEntity(AddExpenseViewModel model, string picturePath)
        {
            return new ExpenseEntity
            {
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(model.ExpenseTypeId),
                Details = model.Details,
                Value = model.Value,
                PicturePath = picturePath,
            };
        }
    }
}
