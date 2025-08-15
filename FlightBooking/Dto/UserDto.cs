using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBooking.Dto
{
    public class UserDto
    {
        public int UserID { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }
    }

    public class UserRegisterDto
    {
        
         [Required(ErrorMessage = "Name is required.")]
         [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
         public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, ErrorMessage = "Phone number must be exactly 10 digits.", MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? Phone { get; set; }


        [Required(ErrorMessage = "Password is required.")]
         [StringLength(15, ErrorMessage = "Password must be alteat 6 characters and 15 characters max.", MinimumLength = 6)]
        public string Password { get; set; } = null!;
        
        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("User|Admin", ErrorMessage = "Role must be either 'Admin', or 'User'.")]
        public required string ROLE { get; set; } 

    }

     public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]

        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;
        

    }

     public class UserUpdateDto
    {
        public int UserID { get; set; }

        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]

        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, ErrorMessage = "Phone number must be exactly 10 digits.", MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? Phone { get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;

        
    }
}