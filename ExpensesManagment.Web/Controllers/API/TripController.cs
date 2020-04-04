using ExpensesManagment.Common.Models;
using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly ITripHelper _tripHelper;
        private readonly IImageHelper _imageHelper;

        public TripController(DataContext dataContext,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            ITripHelper tripHelper,
            IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _tripHelper = tripHelper;
            _imageHelper = imageHelper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TripEntity tripEntity = await _dataContext.Trips
                .Include(t => t.Expenses)
                .ThenInclude(t => t.ExpenseType)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tripEntity == null)
            {
                return BadRequest("Trip doesn't exist.");
            }

            return Ok(_converterHelper.ToTripResponse(tripEntity));
        }

        [HttpPost]
        [Route("GetTrips")]
        public async Task<IActionResult> GetTrips([FromBody] MyTripsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<TripEntity> trips = await _dataContext.Trips
                .Include(t => t.Expenses)
                .ThenInclude(t => t.ExpenseType)
                .Include(t => t.User)
                .Where(t => t.User.Id == request.UserId &&
                            t.StartDate >= request.StartDate &&
                            t.StartDate <= request.EndDate)
                .ToListAsync();
            
            if (trips == null)
            {
                return BadRequest("User doesn't have any trip");
            }

            return Ok(_converterHelper.ToTripResponse(trips));
        }

        [HttpPost]
        [Route("AddTrip")]
        public async Task<IActionResult> AddTrip([FromBody] TripRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dataContext.Add(await _converterHelper.ToTripEntity(request));
            await _dataContext.SaveChangesAsync();

            UserEntity userEntity = await _userHelper.GetUserAsync(request.UserEmail);

            MyTripsRequest myTripsRequest = new MyTripsRequest
            {
                UserId = userEntity.Id,
                StartDate = DateTime.Parse("2020-01-01"),
                EndDate = DateTime.Parse("2021-01-01")
            };

            List<TripEntity> trips = await _dataContext.Trips
               .Include(t => t.Expenses)
               .ThenInclude(t => t.ExpenseType)
               .Include(t => t.User)
               .Where(t => t.User.Id == myTripsRequest.UserId &&
                           t.StartDate >= myTripsRequest.StartDate &&
                           t.StartDate <= myTripsRequest.EndDate)
               .ToListAsync();


            return Ok(_converterHelper.ToTripResponse(trips));
        }

        [HttpPost]
        [Route("AddExpense")]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dataContext.Add(await _converterHelper.ToExpenseEntity(request));
            await _dataContext.SaveChangesAsync();

            TripEntity tripEntity = await _tripHelper.GetTripAsync(request.TripId);
            
            return Ok(_converterHelper.ToTripResponse(tripEntity));
        }

        [HttpPut]
        public async Task<IActionResult>ModifyTrip([FromBody] TripRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TripEntity tripEntity = await _tripHelper.GetTripAsync(request.Id);

            if (tripEntity == null)
            {
                return BadRequest("Trip doesn't exist");
            }

            tripEntity.CityVisited = request.CityVisited;
            tripEntity.StartDate = request.StartDate;
            tripEntity.EndDate = request.EndDate;

            _dataContext.Trips.Update(tripEntity);
            await _dataContext.SaveChangesAsync();
            
            TripEntity updatedTrip = await _tripHelper.GetTripAsync(request.Id);
            return Ok(_converterHelper.ToTripResponse(updatedTrip));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteTrip([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TripEntity tripEntity = await _tripHelper.GetTripAsync(id);

            if (tripEntity == null)
            {
                return BadRequest("Trip doesn't exist");
            }

            _dataContext.Trips.Remove(tripEntity);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        [Route("ModifyExpense")]
        public async Task<IActionResult> ModifyExpense([FromBody] ExpenseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ExpenseEntity expenseEntity = await _tripHelper.GetExpenseAsync(request.Id);

            if (expenseEntity == null)
            {
                return BadRequest("Expense doesn't exist");
            }

            string picturePath = expenseEntity.PicturePath;
            if (request.PictureArray != null && request.PictureArray.Length > 0)
            {
                picturePath = _imageHelper.UploadImage(request.PictureArray, "Expenses");
            }

            expenseEntity.Details = request.Details;
            expenseEntity.Date = request.Date;
            expenseEntity.Value = request.Value;
            expenseEntity.PicturePath = picturePath;

            _dataContext.Expenses.Update(expenseEntity);
            await _dataContext.SaveChangesAsync();

            ExpenseEntity expenseUpdated = await _tripHelper.GetExpenseAsync(request.Id);
            return Ok(_converterHelper.ToExpenseResponse(expenseUpdated));
        }

        [HttpDelete]
        [Route("DeleteExpense/{id}")]
        public async Task<IActionResult> DeleteExpense([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ExpenseEntity expenseEntity = await _tripHelper.GetExpenseAsync(id);

            if (expenseEntity == null)
            {
                return BadRequest("Expense doesn't exist");
            }

            _dataContext.Expenses.Remove(expenseEntity);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
