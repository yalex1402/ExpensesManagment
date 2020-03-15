using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Expenses.OrderBy(e => e.ExpenseName).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ExpenseEntity model = await _dataContext.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

    }
}
