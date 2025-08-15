using FlightBooking.Dto;

namespace FlightBooking.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingInfoDto>> GetAllBookingsAsync();
        Task<GetBookingDto> GetBookingByIdAsync(int id);
        Task BookAsync(BookingDto data);
       
        Task DeleteBookingAsync(int id);

        Task CancelBookingAsync(int bookingId);

        Task<IEnumerable<BookingInfoDto>> GetAllBookingsByUserId(int userId);

        Task<int> getBookingsCount();
        
    }
}