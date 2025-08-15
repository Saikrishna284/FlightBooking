
namespace FlightBooking.Models;

public partial class Booking
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

    public virtual Flight? Flight { get; set; }

    public virtual ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();

    public virtual Payment? Payment { get; set; }

    public virtual User? User { get; set; }
}
