using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBooking.Exceptions
{
    public class CheckinAlreadyCompletedException : Exception
    {
        public CheckinAlreadyCompletedException(string message): base(message)
        {
            
        }
    }
}