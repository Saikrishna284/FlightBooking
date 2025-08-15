using System.ComponentModel.DataAnnotations;


namespace FlightBooking.Dto
{
    public class PassengerDto
    {
    
       [Required(ErrorMessage = "Name is required.")]
       public string Name { get; set; } = null!;

       [Required(ErrorMessage = "Age is required.")]
       [Range(3, 120, ErrorMessage = "Age must be between 3 and 100.")]
        public int AGE { get; set; }
    }

    public class GetPassengerDto
    {
         public int PassengerId { get; set; }

        public int BookingId { get; set; }

        public string Name { get; set; } = null!;

        public int Age { get; set; }

        public int SeatNumber { get; set; }


    }

    
}