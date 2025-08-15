using FlightBooking.Models;

namespace FlightBooking.Interface
{
    public interface IUser : IGenericRepository<User>
    {
        Task<User?> GetUserByEmail(string email);

        Task SuccessfulRegestrationEmail(User user);

        
        
    }
}