

namespace FlightBooking.Models;

public partial class CheckIn
{
    public int CheckInId { get; set; }

    public int? PassengerId { get; set; }

    public DateTime? CheckInDate { get; set; }

    public virtual Passenger? Passenger { get; set; }
}
