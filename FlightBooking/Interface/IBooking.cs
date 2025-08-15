using FlightBooking.Models;

namespace FlightBooking.Interface
{
    public interface IBooking : IGenericRepository<Booking>
    {
         Task CancelBooking(int bookingId);
         Task<List<int>> GenerateSeat(int bookingId, string Seatclass);

         Task SendConfirmationEmail(Booking bookingdata);

         Task<string> PassengerNames(int bookingId);

         Task<string> PassengerSeats(int bookingId);

          Task<string> PassengerId(int bookingId);
          Task<IEnumerable<Booking>> GetBookingsByuserIdAsync(int userId);

          Task<IEnumerable<Booking>> GetBookingsAsync();

    }
}