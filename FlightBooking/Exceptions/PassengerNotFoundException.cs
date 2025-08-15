using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBooking.Exceptions
{
    public class PassengerNotFoundException : Exception
    {
        public PassengerNotFoundException(string message) : base(message)
        {
            
        }
    }
}