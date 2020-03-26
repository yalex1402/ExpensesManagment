﻿using ExpensesManagment.Web.Data;
using ExpensesManagment.Web.Data.Entities;
using ExpensesManagment.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesManagment.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;

        public TripController(DataContext dataContext,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip ([FromRoute]int id)
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
    }
}
