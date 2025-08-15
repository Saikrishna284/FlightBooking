using FlightBooking.Dto;
using FlightBooking.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }

        [Authorize(Roles = "User")]

        [HttpPost("Book")]

        public async Task<IActionResult> Book([FromBody]BookingDto data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                await _service.BookAsync(data);
                return Ok(new { message = "Flight booked successfully" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
           catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal Server Error: {ex.Message}" });
            }
                    
        }

         [Authorize(Roles = "Admin")]
        
        [HttpDelete("DeleteBooking/{bookingId}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] int bookingId)
        {
            try
            {
                await _service.DeleteBookingAsync(bookingId);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch(Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the booking." });
            }
        }

        [Authorize(Roles = "Admin")]
        
        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var allBookings = await _service.GetAllBookingsAsync();
                if(allBookings.Any())
                {
                    return Ok(allBookings);
                }

                return NotFound(new { message = "No Bookings were found." });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }
           
        }

        [Authorize(Roles = "Admin, User")]

        [HttpGet("GetBookingById/{bookingId}")]

        public async Task<IActionResult> GetBookingById([FromRoute] int bookingId)
        {
            try
            {
                var booking = await _service.GetBookingByIdAsync(bookingId);
                return Ok(booking);
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

        [Authorize(Roles = "User")]

        [HttpPut("CancelBooking/{bookingId}")]

        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            try
            {
                await _service.CancelBookingAsync(bookingId);
                return Ok(new { message = "Booking cancelled successfully." });
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch(Exception)
            {
                return StatusCode(500, new { message = "An error occurred while canceling the booking."  });
            }
            
            
        }
    
    
    [Authorize(Roles = "User")]

    [HttpGet("GetBookingsByUserId/{userId}")]

        public async Task<IActionResult> GetBookingsByUserId([FromRoute] int userId)
        {
            try
            {
                var bookings = await _service.GetAllBookingsByUserId(userId);
                return Ok(bookings);
            }
            catch(Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the record." });
            }
            
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("GetTotalBookings")]
         public async Task<IActionResult> GetTotalBookings()
        {
            try
            {
                var bookingsCount = await _service.getBookingsCount();
                return Ok(new {totalCount = bookingsCount});
               
            }
            catch(Exception ex)
            {
               return StatusCode(500,  new { message = "An internal server error occurred.", error = ex.Message });
            }

        }
    }

}