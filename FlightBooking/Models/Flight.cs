

namespace FlightBooking.Models;

public partial class Flight
{
    public int FlightId { get; set; }

    public string? FlightNumber { get; set; } = null!;

    public string? AirlineName { get; set; } = null!;

    public string SourceCity { get; set; } = null!;

    public string DestinationCity { get; set; } = null!;

    public DateTime DepartureDateTime { get; set; }

    public DateTime ArrivalDateTime { get; set; }

    public int AvailableSeats { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
