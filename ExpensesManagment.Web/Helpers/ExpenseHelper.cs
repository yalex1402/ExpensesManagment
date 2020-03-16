﻿using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Helpers
{
    public class ExpenseHelper : IExpenseHelper
    {
        private readonly DataContext _dataContext;

        public ExpenseHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ExpenseTypeEntity> GetExpenseTypeAsync(string name)
        {
            return await _dataContext.ExpenseTypes.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<ExpenseTypeEntity> GetExpenseTypeAsync(int id)
        {
            return await _dataContext.ExpenseTypes.FindAsync(id);
        }
    }
}