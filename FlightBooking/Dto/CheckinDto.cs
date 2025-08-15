
using System.ComponentModel.DataAnnotations;


namespace FlightBooking.Dto
{
    public class CheckinDto
    {

        [Required(ErrorMessage = "Passenger Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Passenger ID must be greater than 0.")]
         public int PassengerId { get; set; }
    }

    public class GetCheckinDto
    {
        public int CheckInId { get; set; }

        public int? PassengerId { get; set; }

        public DateTime? CheckInDate { get; set; }

    }
}