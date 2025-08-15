
using System.ComponentModel.DataAnnotations;


namespace FlightBooking.Dto
{
    public class BookingDto
    {
       
        [Required(ErrorMessage = "User ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0.")]
        public int UserID { get; set; }
        
        [Required(ErrorMessage = "flight ID is required.")]
         [Range(1, int.MaxValue, ErrorMessage = "Flight ID must be greater than 0.")]
        public int FlightID { get; set; }
        
        [Required(ErrorMessage = "Class is required.")]
        [RegularExpression("Economy|Business|First Class", ErrorMessage = "Class must be either 'Economy', 'Business', or 'First Class'.")]
        public string Class { get; set; } = null!;

        [Required(ErrorMessage = "Number of tickets is required.")]
        [Range(1, 10, ErrorMessage = "Number of tickets must be between 1 and 10.")]
        public int NoOfTickets { get; set; }
        
        [Required(ErrorMessage = "At least one passenger is required.")]
        [MinLength(1, ErrorMessage = "At least one passenger must be added.")]
        public virtual ICollection<PassengerDto> Passengers { get; set; } = new List<PassengerDto>();

        public required virtual PaymentDto Payment { get; set; }
        
    }

    public class GetBookingDto
    {
        public int BookingId { get; set; }

        public int? UserId { get; set; }

        public int FlightId { get; set; }

        public int? PaymentId { get; set; }

        public string Class { get; set; } = null!;

        public DateTime? BookingDate { get; set; }

        public int NoOfTickets { get; set; }

        public decimal? Fare { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }

        
        
    }

    public  class BookingInfoDto
    {
        public int BookingId { get; set; }

        public int? UserId { get; set; }

        public int FlightId { get; set; }

        public int? PaymentId { get; set; }

        public string Class { get; set; } = null!;

        public DateTime? BookingDate { get; set; }

        public int NoOfTickets { get; set; }

        public decimal? Fare { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }

        public virtual FlightDto? Flight { get; set; }

        public virtual ICollection<GetPassengerDto> Passengers { get; set; } = new List<GetPassengerDto>();

        public virtual GetPaymentDto? Payment { get; set; }

        public virtual UserDto? User { get; set; }
    } 

    

}