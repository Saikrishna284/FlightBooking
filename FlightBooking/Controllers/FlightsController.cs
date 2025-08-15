using FlightBooking.Dto;
using FlightBooking.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _service;

       public FlightsController(IFlightService service)
        {
           _service = service;
        }

        [Authorize(Roles = "Admin, User")]

        [HttpGet("GetAllFlights")]

        public async Task<IActionResult> GetAllFlights()
        {
            try
            {
                var response = await _service.GetAllFlightsAsync();
            
                if(response.Any())
                {
                    return Ok(response);
                }
               
                return NotFound(new {message = "No flights were found!"});
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error : {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet("GetFlightById/{id}")]

        public async Task<IActionResult> GetFlightById([FromRoute] int id)
        {
            try
            {
                var response = await _service.GetFlightByIdAsync(id);
                return Ok(response);
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

        [HttpPost("AddNewFlight")]

        public async Task<IActionResult> AddNewFlight(AddFlightDto newFlight)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.Equals(newFlight.SourceCity, newFlight.DestinationCity, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { message = "Source and Destination cannot be the same!" });
                }
                DateTime currentIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                if (newFlight.DepartureDateTime < currentIndianTime)
                {
                    return BadRequest(new { message = "Departure date cannot be past" });
                }

                if (newFlight.DepartureDateTime > newFlight.ArrivalDateTime )
                {
                    return BadRequest(new { message = "Arrival date must be greater than Departure date" });
                }

                 if (newFlight.DepartureDateTime == newFlight.ArrivalDateTime )
                {
                    return BadRequest(new { message = "Arrival date Departure date Cannot be same" });
                }


                await _service.AddFlightAsync(newFlight);
                return Ok(new {message = "Flight Added Successfully"});
            } 
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal server error occurred.", error = ex.Message });
            }

            
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateFlight/{id}")]

        public async Task<IActionResult> UpdateFlight([FromRoute] int id, [FromBody] UpdateFlightDto flight)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.Equals(flight.SourceCity, flight.DestinationCity, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { message = "Source and Destination cannot be the same!" });
                }
                DateTime currentIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                if (flight.DepartureDateTime < currentIndianTime)
                {
                    return BadRequest(new { message = "Departure date cannot be past" });
                }

                if (flight.DepartureDateTime > flight.ArrivalDateTime )
                {
                    return BadRequest(new { message = "Arrival date must be greater than Departure date" });
                }

                if (flight.DepartureDateTime == flight.ArrivalDateTime )
                {
                    return BadRequest(new { message = "Arrival date Departure date Cannot be same" });
                }

                if(id != flight.FlightID)
                {
                    return BadRequest(new { message = "Flight Id Did not match!"});
                }


                await _service.UpdateFlightAsync(flight);
                return Ok(new {message = "Flight Updated Successfully"});
            }
            catch(KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message  = ex.Message });
            } 

        }

       
           
        [Authorize(Roles = "Admin")]

        [HttpDelete("DeleteFlight/{id}")]

        public async Task<IActionResult> DeleteFlight([FromRoute] int id)
        {
            try
            {
                await _service.DeleteFlightAsync(id);
                return Ok(new {message = "Flight Deleted Successfully"});
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new {message = ex.Message});
            }
            catch(Exception ex)
            {
                return StatusCode(500,  new { message = "An internal server error occurred.", error = ex.Message });
            }
            
        }

        //[Authorize(Roles = "Admin, User")]

        [HttpGet("SearchFlights")]

       public async Task<IActionResult> SearchFlights(string source, string destination, DateTime date)
       {
            try
            {
                
                if (string.IsNullOrWhiteSpace(source))
                {
                    return BadRequest(new { message = "Source cannot be empty or null" });
                }

               
                if (string.IsNullOrWhiteSpace(destination))
                {
                    return BadRequest(new { message = "Destination cannot be empty or null" });
                }

               
                if (string.Equals(source, destination, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { message = "Source and Destination cannot be the same!" });
                }

              
                if (date < DateTime.UtcNow.Date)
                {
                    return BadRequest(new { message = "Departure date must be present or in the future" });
                }

               
                var response = await _service.SearchFlights(source, destination, date);
                return Ok(response);
                

              
            }
            catch(KeyNotFoundException ex)
            {
               return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while searching for flights.", details = ex.Message });
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetTotalFlights")]

        public async Task<IActionResult> GetTotalFlights()
        {
            try
            {
                var flightsCount = await _service.getFlightsCount();
                return Ok(new {totalCount = flightsCount});
               
            }
            catch(Exception ex)
            {
               return StatusCode(500,  new { message = "An internal server error occurred.", error = ex.Message });
            }

        }

      
    }
}