using ExpensesManagment.Common.Enums;
using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
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

        public TripController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Trips
                .Include(t => t.Expenses)
                .OrderBy(t => t.StartDate)
                .ToListAsync());
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

            List<TripEntity> tripEntity = await _dataContext.Trips
                .Include(t=> t.Expenses)
                .Where(t => t.User.Id == id)
                .OrderBy(t=> t.StartDate)
                .ToListAsync();

            if (tripEntity == null)
            {
                return NotFound();
            }

            return View(tripEntity);
        }

    }
}
