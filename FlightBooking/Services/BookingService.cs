using AutoMapper;
using FlightBooking.Dto;
using FlightBooking.Interface;
using FlightBooking.Models;


namespace FlightBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBooking _repository;

        
        private readonly IPassengerService _service;
        private readonly IMapper _mapper;
        public BookingService(IBooking repository, IMapper mapper, IPassengerService service)
        {
            _repository = repository;
            _mapper = mapper;
            _service = service;

        }

    public async Task BookAsync(BookingDto data)
    {
        try
        {
          
            if (data?.Payment == null)
            {
                throw new ArgumentException("Payment details are missing");
            }

            if (data.Payment.PaymentStatus != "Paid")
            {
                throw new InvalidOperationException("Payment is not completed.");
            }

            var bookingData = _mapper.Map<Booking>(data);
            decimal fare = CalculateFare(bookingData.Class);
            bookingData.Fare = fare;
            bookingData.TotalAmount = fare * bookingData.NoOfTickets;
             
            DateTime indianConvertedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            bookingData.BookingDate = indianConvertedTime;
            bookingData.Status = "Confirmed";
            bookingData.Payment!.AmountPaid = fare * bookingData.NoOfTickets;

            var AvailableSeats = await _repository.GenerateSeat(bookingData.FlightId, bookingData.Class);
            foreach (var passenger in bookingData.Passengers)
            {
                passenger.SeatNumber = AvailableSeats.Min();
                AvailableSeats.Remove(AvailableSeats.Min());
            }

            await _repository.AddAsync(bookingData);
            await _repository.SendConfirmationEmail(bookingData);
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while booking", ex);
        }
    }
        
           

        public async Task DeleteBookingAsync(int id)
        {
           
            await _repository.DeleteAsync(id);
          
        }
            
        

        public async Task<IEnumerable<BookingInfoDto>> GetAllBookingsAsync()
        {
            try
            {
                //var allBookings = await _repository.GetAllAsync();
                var allBookings = await _repository.GetBookingsAsync();
                return _mapper.Map<IEnumerable<BookingInfoDto>>(allBookings);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving bookings.", ex);
            }
        }

        public async Task<GetBookingDto> GetBookingByIdAsync(int id)
        {
            var booking = await _repository.GetByIdAsync(id);
            var bookingMap = _mapper.Map<GetBookingDto>(booking);
            return bookingMap;
        }

       public async Task CancelBookingAsync(int bookingId)
      {
            try
            {
                await _repository.CancelBooking(bookingId);
               
            }
            catch (KeyNotFoundException)
            {
                throw;  
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while canceling the booking.", ex);
            }
   }


    public decimal CalculateFare(String classType)
        {
            if(string.Equals(classType, "Economy", StringComparison.OrdinalIgnoreCase))
            {
                return 3000;
            }
            else if(string.Equals(classType, "First Class", StringComparison.OrdinalIgnoreCase))
            {
                return 9000;
            }
            else if(string.Equals(classType, "Business", StringComparison.OrdinalIgnoreCase))
            {
                return 6000;
            }
            else
            {
                return 0;
            }
        }

        public async Task<IEnumerable<BookingInfoDto>> GetAllBookingsByUserId(int userId)
        { try
            {
                var allBookings = await _repository.GetBookingsByuserIdAsync(userId);
                return _mapper.Map<IEnumerable<BookingInfoDto>>(allBookings);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving bookings.", ex);
            }

            
        }

        
        public async Task<int> getBookingsCount()
        {
            try
            {
                var totalBookings = await _repository.CountAsync();
                return totalBookings;
                
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching the total Bookings.", ex);
            }

        }
    }
}