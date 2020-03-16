using ExpensesManagment.Common;
using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Helpers;
using ExpensesManagment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IExpenseHelper _expenseHelper;
        private readonly IConverterHelper _converterHelper;

        public ExpensesController(DataContext dataContext,
            ICombosHelper combosHelper,
            IImageHelper imageHelper,
            IExpenseHelper expenseHelper,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
            _imageHelper = imageHelper;
            _expenseHelper = expenseHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Expenses.Include(t => t.ExpenseType)
                .OrderBy(e => e.Value).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ExpenseEntity model = await _dataContext.Expenses.Include(t => t.ExpenseType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public IActionResult Create()
        {
            AddExpenseViewModel model = new AddExpenseViewModel
            {
                ExpensesType = _combosHelper.GetComboExpenses()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                if (model.PictureFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "Expenses");
                }
                ExpenseEntity expense = await _converterHelper.ToExpenseEntity(model, path);
                _dataContext.Add(expense);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.ExpensesType = _combosHelper.GetComboExpenses();
            return View(model);
        }
    }
}
