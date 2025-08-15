using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBooking.Dto;
using FlightBooking.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _service;

        public PassengerController(IPassengerService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllPassengers")]

        public async Task<IActionResult> GetAllPassengers()
        {
            try
            {
                var passengers = await _service.GetAllPassengers();
                if(passengers.Any())
                {
                    return Ok(passengers);
                }

                return NotFound(new {message = "No Passengers found!"});
               

            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }
           
           
        }

        [Authorize(Roles = "Admin, User")]

        [HttpGet("GetPassengerById/{passengerId}")]

        public async Task<IActionResult> GetPassengerById([FromRoute] int passengerId)
        {
            try
            {
                 var passenger = await _service.GetPassengerByIdAsync(passengerId);
                 return Ok(passenger);

            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch(Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the record." });
            }
        }

        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllPassengersByFlightId/{flightId}")]

        public async Task<IActionResult> GetAllPassengersByFlightId(int flightId)
        {
            try
            {
                var passengers = await _service.GetPassengersByFlightIdAsync(flightId);
                if(passengers.Any())
                {
                    return Ok(passengers);
                }

                return NotFound(new {message = "No Passengers found!"});
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }

            
        }

        

       
    }
}