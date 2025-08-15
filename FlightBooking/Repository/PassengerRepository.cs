using FlightBooking.Data;
using FlightBooking.Interface;
using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.Repository
{
    public class PassengerRepository : GenericRepository<Passenger>, IPassenger
    {
        public PassengerRepository(FlightDbContext context) : base (context) { }

        public async Task<IEnumerable<Passenger>> GetPassengersByFlightId(int flightId)
        {
            var passengers = await _context.Passengers.Where(p => p.Booking!.FlightId == flightId).ToListAsync();
            return passengers;
        }
        

    }
}