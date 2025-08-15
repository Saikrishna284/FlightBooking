using System.ComponentModel.DataAnnotations;

namespace FlightBooking.Dto
{
    public class FlightDto
    {
       
        public int FlightID { get; set; }
        
        public string FlightNumber { get; set; } = null!;

        public string AirlineName { get; set; } = null!;

        public string SourceCity { get; set; } = null!;

        public string DestinationCity { get; set; } = null!;

        public DateTime DepartureDateTime { get; set; }

        public DateTime ArrivalDateTime { get; set; }

        //public int AvailableSeats { get; set; }

    
    }

     public class AddFlightDto
    {
        [Required(ErrorMessage = "Flight number is required.")]
        public string FlightNumber { get; set; } = null!;
        
        [Required(ErrorMessage = "Airline is required.")]
        public string AirlineName { get; set; } = null!;
        
        [Required(ErrorMessage = "Source City is required.")]
        public string SourceCity { get; set; } = null!;
        [Required(ErrorMessage = "Destination city is required.")]
        public string DestinationCity { get; set; } = null!;
        
        [Required(ErrorMessage = "Departure date and time is required.")]
        public DateTime DepartureDateTime { get; set; }

        [Required(ErrorMessage = "Arrival date and time is required.")]
        public DateTime ArrivalDateTime { get; set; }
        
        [Required(ErrorMessage = "Available seats are required.")]
        [Range(1, 180, ErrorMessage = "Available seats must be greater than 0.")]
        public int AvailableSeats { get; set; }

    }

    public class UpdateFlightDto
    {
       
        [Required(ErrorMessage = "Flight Id is required.")]
        [Range(1, int.MaxValue,ErrorMessage = "Flight Id must be greater than zero.")]
         public int FlightID { get; set; }

        [Required(ErrorMessage = "Airline is required.")]
        public string AirlineName { get; set; } = null!;
        
        [Required(ErrorMessage = "Source City is required.")]
        public string SourceCity { get; set; } = null!;
        [Required(ErrorMessage = "Destination city is required.")]
        public string DestinationCity { get; set; } = null!;
        
        [Required(ErrorMessage = "Departure date and time is required.")]
        public DateTime DepartureDateTime { get; set; }

        [Required(ErrorMessage = "Arrival date and time is required.")]
        public DateTime ArrivalDateTime { get; set; }
        
        [Required(ErrorMessage = "Available seats are required.")]
        [Range(1, 180, ErrorMessage = "Available seats must be greater than 0.")]
        public int AvailableSeats { get; set; }

    }
}