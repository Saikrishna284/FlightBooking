using FlightBooking.Models;


namespace FlightBooking.Interface
{
    public interface IFlight : IGenericRepository<Flight>
    {
        Task<IEnumerable<Flight>> SearchFlights(string source, string destination, DateTime date);
    }
}