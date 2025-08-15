using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBooking.Data;
using FlightBooking.Interface;
using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.Repository
{
    public class FlightRepository : GenericRepository<Flight>, IFlight
    {
        
       public FlightRepository(FlightDbContext context) : base(context) { }

        public async Task<IEnumerable<Flight>> SearchFlights(string source, string destination, DateTime date)
        {
            var flights = await _context.Flights.
                          Where(f => f.SourceCity == source && 
                          f.DestinationCity == destination && 
                          f.DepartureDateTime.Date == date.Date ).ToListAsync();
            return flights;
        }

    
    }
}

