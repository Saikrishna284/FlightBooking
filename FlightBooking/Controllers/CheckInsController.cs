using FlightBooking.Dto;
using FlightBooking.Exceptions;
using FlightBooking.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private readonly ICheckinService _service;

        public CheckInsController(ICheckinService service)
        {
            _service = service;
        }

        [Authorize(Roles = "User")]

        [HttpPost("PassengerCheckin")]

        public async Task<IActionResult> PassengerCheckIn([FromBody]CheckinDto data)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                 await _service.Checkin(data);
                 return Ok("Checkin Completed Successfully");
            }
            catch (PassengerNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (CheckinAlreadyCompletedException ex)
            {
                return BadRequest(new { message = ex.Message });
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

        [HttpGet("GetAllChekins")]

        public async Task<IActionResult> GetAllCheckins()
        {
            try
            {
                var checkins = await _service.GetAllCheckinsAsync();
                if(checkins.Any())
                {
                    return Ok(checkins);
                }

                return NotFound(new {message = "No Checkins found!"});
               

            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin, User")]

        [HttpGet("GetCheckinById/{id}")]

        public async Task<IActionResult> GetCheckinById(int id)
        {
            try
            {
                 var checkin = await _service.GetCheckinByIdAsync(id);
                 return Ok(checkin);

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
    }
}