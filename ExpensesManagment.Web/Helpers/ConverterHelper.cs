﻿
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
        public async Task<ExpenseEntity> ToAddExpenseEntity(ExpenseViewModel model, string picturePath)
        {
            return new ExpenseEntity
            {
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(model.ExpenseId),
                Details = model.Details,
                Value = model.Value,
                Date = model.Date,
                PicturePath = picturePath,
            };
        }

        public async Task<ExpenseEntity> ToEditExpenseEntity(ExpenseViewModel model, string picturePath)
        {
            return new ExpenseEntity
            {
                Id = model.Id,
                ExpenseType = await _expenseHelper.GetExpenseTypeAsync(model.ExpenseId),
                Details = model.Details,
                Value = model.Value,
                Date = model.Date,
                PicturePath = picturePath,
            };
        }
    }
}
