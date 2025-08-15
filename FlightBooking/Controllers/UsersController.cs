using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FlightBooking.Dto;
using FlightBooking.Exceptions;
using FlightBooking.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

       public UsersController(IUserService service)
        {
           _service = service;
        }

        

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
        {
            try
           {
               if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _service.Register(user);
                return Ok(new { message = "User created successfully" });
           }
           catch(AccountAlreadyExistsExcception ex)
           { 
                return BadRequest(new { message = ex.Message });
           }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message} );
            }
           
            
        }

       

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _service.Login(user);
                if(response.Equals("Invalid"))
                {
                    return Unauthorized(new { message = "Invalid Credentials!" });
                }
                return Ok(new {token = response });
            }
            catch(Exception ex)
            {
               return StatusCode(500, new {message = ex.Message});
            }
           
           
        }

        [Authorize(Roles = "Admin")]

        [HttpGet("GetAllUsers")]

        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _service.GetAllUsersAsync();
                if(users.Any())
                {
                    return Ok(users);
                } 
                    
                return NotFound(new { message = "No users were found." });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error {ex.Message}");
            }
           
        }

        [Authorize(Roles = "User")]

        [HttpDelete("DeleteUser/{id}")]

        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                await _service.DeleteUserAsync(id);
                return Ok("Account deleted Successfully");
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

        [Authorize(Roles = "Admin, User")]

        [HttpGet("GetUserById/{id}")]

        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            try
            {
                var user = await _service.GetUserByIdAsync(id);
                return Ok(user);
              
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

        [Authorize(Roles = "Admin, User")]

        [HttpPut("UpdateUser/{id}")]

        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserUpdateDto user)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _service.UpdateUserAsync(user);
                return Ok(new {message = "Details Updated Successfully"});
               
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

        [Authorize(Roles = "Admin, User")]

        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
        {
            try
            {
                 string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; 
                 if(!Regex.IsMatch(email, emailPattern))
                 {
                    return BadRequest("Invalid email");
                 }
                var user = await _service.findUserByEmail(email);
                return Ok(user);

            }
            catch(UserNotFoundException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetTotalUsers")]

        public async Task<IActionResult> GetTotalUsers()
        {
            try
            {
                var usersCount = await _service.getUsersCount();
                return Ok(new {totalCount = usersCount});
               
            }
            catch(Exception ex)
            {
               return StatusCode(500,  new { message = "An internal server error occurred.", error = ex.Message });
            }

        }

        
        
        

        
    }
}