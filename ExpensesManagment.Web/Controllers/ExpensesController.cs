using ExpensesManagment.Web.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly DataContext _dataContext;

        public ExpensesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
