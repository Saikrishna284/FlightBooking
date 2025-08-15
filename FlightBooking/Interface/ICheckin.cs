using FlightBooking.Models;

namespace FlightBooking.Interface
{
    public interface ICheckin : IGenericRepository<CheckIn>
    {
        Task<bool> passengerVerification(int passengerId);
        
    }
}