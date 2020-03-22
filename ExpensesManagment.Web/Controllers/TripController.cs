using ExpensesManagment.Common.Enums;
using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Helpers;
using ExpensesManagment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Controllers
{
    public class TripController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;

        public TripController(DataContext dataContext,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
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
                TripEntity tripEntity = await _converterHelper.ToTripEntity(model);
                _dataContext.Add(tripEntity);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(UserTripDetail)}/{model.UserId}");
            }
            return View(model);
        }
    }
}
