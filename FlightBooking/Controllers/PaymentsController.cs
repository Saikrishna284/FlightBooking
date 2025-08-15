using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBooking.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        
        private readonly IPaymentService _service;

       public PaymentsController(IPaymentService service)
        {
           _service = service;
        }

        [Authorize(Roles = "Admin")]
        
        [HttpGet("GetAllPaymentDetails")]

        public async Task<IActionResult> GetAllPaymentDetails()
        {
                
           try
            {
                var payments = await _service.GetAllPaymentsAsync();
                if(payments.Any())
                {
                    return Ok(payments);
                }

                return NotFound(new {message = "No Payments found!"});
               

            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }
                

        }

        [Authorize(Roles = "Admin, User")]

        [HttpGet("GetPaymentDetailsById/{paymentId}")]

        public async Task<IActionResult> GetPaymentDetailsById([FromRoute] int paymentId)
        {
            try
            {
                 var payment = await _service.GetPaymentByIdAsync(paymentId);
                 return Ok(payment);

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
        [HttpGet("GetPaymentsSum")]

        public async Task<IActionResult> GetPaymentsSum()
        {
            try
            {
                var totalAmount = await _service.paymentsTotalSum();
                return Ok(new {totalCount = totalAmount});
               
            }
            catch(Exception ex)
            {
               return StatusCode(500,  new { message = "An internal server error occurred.", error = ex.Message });
            }

        }
    }
}