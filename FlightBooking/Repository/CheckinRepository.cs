using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using FlightBooking.Data;
using FlightBooking.Exceptions;
using FlightBooking.Interface;
using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.Repository
{
    public class CheckinRepository : GenericRepository<CheckIn>, ICheckin
    {
        public CheckinRepository(FlightDbContext context) : base(context) { }

        public async Task<bool> passengerVerification(int passengerId)
        {
            var findPassenger = await _context.Passengers.FindAsync(passengerId);
            if(findPassenger == null)
            {
                throw new PassengerNotFoundException("No passenger found  with the given Passenger Id");
            }
            var checkinWithPassengerId = await _context.CheckIns.FirstOrDefaultAsync(c => c.PassengerId == passengerId);
            if(checkinWithPassengerId != null)
            {
                throw new CheckinAlreadyCompletedException("Chekin already completed with this Passenger Id");
            }
            return true;
            
        }
    }
}