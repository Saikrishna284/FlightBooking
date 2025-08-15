using FlightBooking.Models;

namespace FlightBooking.Interface
{
    public interface IPassenger : IGenericRepository<Passenger>
    {
       Task<IEnumerable<Passenger>> GetPassengersByFlightId(int flightId);

       
    }
}