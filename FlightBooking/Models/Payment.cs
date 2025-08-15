
namespace FlightBooking.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public decimal AmountPaid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public virtual Booking? Booking { get; set; }
}
