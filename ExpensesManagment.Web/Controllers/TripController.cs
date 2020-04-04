using ExpensesManagment.Common.Enums;
using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Helpers;
using ExpensesManagment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Controllers
{
    public class TripController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        private readonly ITripHelper _tripHelper;
        private readonly IExpenseHelper _expenseHelper;
        private readonly IImageHelper _imageHelper;
        private readonly ICombosHelper _combosHelper;

        public TripController(DataContext dataContext,
            IConverterHelper converterHelper,
            ITripHelper tripHelper,
            IExpenseHelper expenseHelper,
            IImageHelper imageHelper,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _tripHelper = tripHelper;
            _expenseHelper = expenseHelper;
            _imageHelper = imageHelper;
            _combosHelper = combosHelper;
        }

        public async Task<IActionResult> UserTrip()
        {
            return View(await _dataContext.Users
                .Include(t => t.Trips)
                .ThenInclude(t => t.Expenses)                
                .OrderBy(t => t.UserName)
                .Where(t=> t.UserType != UserType.Admin && t.UserType != UserType.Manager)
                .ToListAsync());
        }

        public async Task<IActionResult> MyTrips()
        {
            UserEntity user = await _dataContext.Users
                .Include(t => t.Trips)
                .ThenInclude(t => t.Expenses)
                .OrderBy(t => t.UserName)
                .FirstOrDefaultAsync(t => t.UserName == User.Identity.Name);

            UserTripDetailViewModel model = await _converterHelper.ToUserTripDetailViewModel(user.Id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> UserTripDetail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserTripDetailViewModel model = await _converterHelper.ToUserTripDetailViewModel(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> TripExpensesDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            TripExpenseViewModel model = await _converterHelper.ToTripExpenseViewModel(id);
            
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            UserEntity userEntity = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if(userEntity == null)
            {
                return NotFound();
            }
            
            TripViewModel model = new TripViewModel
            {
                User = userEntity,
                UserId = userEntity.Id
            };
            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TripViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity userEntity = await _dataContext.Users.FindAsync(model.UserId);
                TripEntity tripEntity = await _converterHelper.ToAddTripEntity(model);
                _dataContext.Add(tripEntity);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(UserTripDetail)}/{model.UserId}");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TripEntity tripEntity = await _dataContext.Trips
                .Include(t => t.User)
                .FirstOrDefaultAsync(tu => tu.Id == id);

            if (tripEntity == null)
            {
                return NotFound();
            }

            _dataContext.Trips.Remove(tripEntity);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(UserTripDetail)}/{tripEntity.User.Id}");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            UserEntity userEntity = await _dataContext.Users
                .Include(u => u.Trips)
                .ThenInclude(u => u.Expenses)
                .FirstOrDefaultAsync(ut => ut.Id == id);

            if (userEntity == null)
            {
                return NotFound();
            }

            _dataContext.Remove(userEntity);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(UserTrip)}");
        }

        public async Task<IActionResult> Edit (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TripEntity tripEntity = await _tripHelper.GetTripAsync(id);

            if (tripEntity == null)
            {
                return NotFound();
            }

            TripViewModel tripViewModel = await _converterHelper.ToTripViewModel(tripEntity);

            return View(tripViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TripViewModel model)
        {
            if (ModelState.IsValid)
            {
                TripEntity tripEntity = await _converterHelper.ToEditTripEntity(model);
                _dataContext.Update(tripEntity);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(UserTripDetail)}/{model.UserId}");
            }
            return View(model);
        }

        public async Task<IActionResult> EditExpense(int? id)
        {
            ExpenseViewModel model = new ExpenseViewModel
            {
                ExpensesType = _combosHelper.GetComboExpenses()
            };

            if (id == null)
            {
                return NotFound();
            }

            ExpenseEntity expense = await _dataContext.Expenses
                .Include(et => et.ExpenseType)
                .Include (et => et.Trip)
                .FirstOrDefaultAsync(e => e.Id == id);

            model.TripId = expense.Trip.Id;
            model.Id = expense.Id;
            model.Details = expense.Details;
            model.PicturePath = expense.PicturePath;
            model.Value = expense.Value;
            model.ExpenseId = expense.ExpenseType.Id;
            model.LogoPath = expense.ExpenseType.LogoPath;

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditExpense(ExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                if (model.PictureFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "Expenses");
                    model.PicturePath = path;
                }

                ExpenseEntity expense = await _converterHelper.ToEditExpenseEntity(model, model.PicturePath);
                _dataContext.Update(expense);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(TripExpensesDetail)}/{model.TripId}");
            }
            model.ExpensesType = _combosHelper.GetComboExpenses();
            return View(model);
        }

        public async Task<IActionResult> DeleteExpense(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExpenseEntity expense = await _dataContext.Expenses
                .Include(e => e.Trip)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            _dataContext.Expenses.Remove(expense);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(TripExpensesDetail)}/{expense.Trip.Id}");
        }

        public async Task<IActionResult> DetailsExpense(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ExpenseEntity model = await _dataContext.Expenses
                .Include(t => t.ExpenseType)
                .Include(t => t.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public IActionResult CreateExpense(int id)
        {
            ExpenseViewModel model = new ExpenseViewModel
            {
                TripId = id,
                ExpensesType = _combosHelper.GetComboExpenses()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(ExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;
                if (model.PictureFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.PictureFile, "Expenses");
                }
                model.Date = DateTime.UtcNow;
                ExpenseEntity expense = await _converterHelper.ToAddExpenseEntity(model, path);
                _dataContext.Add(expense);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(TripExpensesDetail)}/{model.TripId}");
            }

            model.ExpensesType = _combosHelper.GetComboExpenses();
            return View(model);
        }
    }
}
