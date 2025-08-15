
namespace FlightBooking.Models;

public partial class Passenger
{
    public int PassengerId { get; set; }

    public int? BookingId { get; set; }

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public int SeatNumber { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual CheckIn? CheckIn { get; set; }
}
