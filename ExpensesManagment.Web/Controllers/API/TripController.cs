﻿using ExpensesManagment.Common.Models;
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

        public TripController(DataContext dataContext,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            ITripHelper tripHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _tripHelper = tripHelper;
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
    }
}
