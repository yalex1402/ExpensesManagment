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

        

    }
}
