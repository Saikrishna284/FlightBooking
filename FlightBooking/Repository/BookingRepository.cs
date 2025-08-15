using System.Net;
using System.Net.Mail;
using FlightBooking.Data;
using FlightBooking.Interface;
using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;


namespace FlightBooking.Repository
{
    public class BookingRepository :  GenericRepository<Booking>, IBooking
    {
        public BookingRepository(FlightDbContext context) : base(context) { }
       
       public async Task<IEnumerable<Booking>> GetBookingsByuserIdAsync(int userId)
       {
          var bookingRecords = await _context.Bookings
              .Include(b => b.Flight)
              .Include(b => b.Passengers)
              .Include(b => b.Payment)
              .Include(b => b.User)
              .Where(b => b.UserId == userId).ToListAsync();
          return bookingRecords;
       }

       public async Task<IEnumerable<Booking>> GetBookingsAsync()
       {
          var bookingRecords = await _context.Bookings
              .Include(b => b.Flight)
              .Include(b => b.Passengers)
              .Include(b => b.Payment)
              .Include(b => b.User)
              .OrderByDescending(b => b.BookingDate) 
              .ToListAsync();
             
          return bookingRecords;
       }
       
        public async Task CancelBooking(int bookingId)
        { 
            
            var bookingRecord = await _context.Bookings.FindAsync(bookingId);
            
            if (bookingRecord == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            bookingRecord.Status = "Cancelled";

            
            var passengersToRemove = await _context.Passengers
                                    .Where(p => p.BookingId == bookingId)
                                    .ToListAsync();
            _context.Passengers.RemoveRange(passengersToRemove);

          
            var paymentRecord = await _context.Payments
                                    .FirstOrDefaultAsync(p => p.PaymentId == bookingRecord.PaymentId);

            if (paymentRecord != null)  
            {
                paymentRecord.PaymentStatus = "Refund";
            }

            await _context.SaveChangesAsync();
            await SendBookingCancelEmail(bookingRecord);
        }


       
         public async Task<List<int>> GenerateSeat(int flightId, string Seatclass)
         {
            List<int> AvailableSeats = new List<int>();
            var seatsBooked = await _context.Passengers
                .Where(p => p.Booking!.FlightId == flightId && p.Booking.Class == Seatclass)
                .Select(p => p.SeatNumber)
                .ToListAsync(); 

            int totalSeatsInClass;
            int seatStartIndex;

            switch (Seatclass)
            {
                case "Economy":
                    totalSeatsInClass = 100;
                    seatStartIndex = 1;
                    break;
                case "Business":
                    totalSeatsInClass = 50;
                    seatStartIndex = 101;
                    break;
                case "First Class":
                    totalSeatsInClass = 30;
                    seatStartIndex = 151;
                    break;
                    
                default:
                    throw new Exception("Invalid class selection.");
            }

            int seatEndIndex = seatStartIndex + totalSeatsInClass - 1;

           
            for (int i = seatStartIndex; i <= seatEndIndex; i++)
            {
                if (!seatsBooked.Contains(i))
                {
                    AvailableSeats.Add(i);
                }
            }

            return AvailableSeats;

            throw new Exception("No available seats in this class."); 
}

        public async Task SendConfirmationEmail(Booking bookingdata)
        {
            var flight = await _context.Flights.FindAsync(bookingdata.FlightId);
            var user = await _context.Users.FindAsync(bookingdata.UserId);
            
            string fromAddress = "saikrishnareddyk28@gmail.com";
            string appPassword = "wuspjkdqbrapahvf";
            string toAddress = user!.Email;

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = "Your Flight Booking Confirmation",
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(toAddress));

            
            message.Body = $@"
                <div style='font-family: Arial, sans-serif; font-size: 14px; color: #333; line-height: 1.6; max-width: 500px;'>
                <p>Dear <strong>{user.Name}</strong>,</p>
                
                <p>Thank you for booking your flight with us! Your reservation has been confirmed. Below are your flight details:</p>
                
                <table style='width: 100%; border-collapse: collapse;'>
                    <tr>
                        <td style='padding: 8px;'><strong>Booking Reference:</strong></td>
                        <td style='padding: 8px;'>{bookingdata.BookingId}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Passenger Name:</strong></td>
                        <td style='padding: 8px;'>{await PassengerNames(bookingdata.BookingId)}</td>
                    </tr>
                     <tr>
                        <td style='padding: 8px;'><strong>Passenger Id:</strong></td>
                        <td style='padding: 8px;'>{await PassengerId(bookingdata.BookingId)}</td>
                    </tr>
                     <tr>
                        <td style='padding: 8px;'><strong>Airline:</strong></td>
                        <td style='padding: 8px;'>{flight!.AirlineName}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Flight:</strong></td>
                        <td style='padding: 8px;'>{flight!.FlightNumber}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Departure:</strong></td>
                        <td style='padding: 8px;'>{flight.SourceCity} - {flight.DepartureDateTime}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Arrival:</strong></td>
                        <td style='padding: 8px;'>{flight.DestinationCity} - {flight.ArrivalDateTime}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Seat:</strong></td>
                        <td style='padding: 8px;'>{await PassengerSeats(bookingdata.BookingId)}</td>
                    </tr>
                    </table>

                    <p>For any assistance, feel free to contact our support team.</p>

                    <p><strong>Safe travels!</strong><br>Super Travels</p>
                </div>";

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(fromAddress, appPassword);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;

                await smtp.SendMailAsync(message);
            }
        }


         public async Task<string> PassengerNames(int bookingId)
         {
            var passengers = await _context.Passengers.Where(p => p.BookingId == bookingId).ToListAsync();
            return string.Join(", ", passengers.Select(p => p.Name));
           
         }

         public async Task<string> PassengerSeats(int bookingId)
         {
            var passengers = await _context.Passengers.Where(p => p.BookingId == bookingId).ToListAsync();
            return string.Join(", ", passengers.Select(p => p.SeatNumber));
         }
         
          public async Task<string> PassengerId(int bookingId)
         {
            var passengers = await _context.Passengers.Where(p => p.BookingId == bookingId).ToListAsync();
            return string.Join(", ", passengers.Select(p => p.PassengerId));
         }

        public async Task SendBookingCancelEmail(Booking bookingdata)
        {
            var flight = await _context.Flights.FindAsync(bookingdata.FlightId);
            var user = await _context.Users.FindAsync(bookingdata.UserId);
            
            string fromAddress = "saikrishnareddyk28@gmail.com";
            string appPassword = "wuspjkdqbrapahvf";
            string toAddress = user!.Email;

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = "Your Flight Booking Confirmation",
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(toAddress));

            message.Body = $@"
            <div style='font-family: Arial, sans-serif; font-size: 14px; color: #333; line-height: 1.6; max-width: 500px;'>
                <p>Dear <strong>{user.Name}</strong>,</p>

                <p>We regret to inform you that your flight booking has been <strong>cancelled</strong> as per your request. Below are the cancellation details:</p>

                <table style='width: 100%; border-collapse: collapse;'>
                    <tr>
                        <td style='padding: 8px;'><strong>Booking Reference:</strong></td>
                        <td style='padding: 8px;'>{bookingdata.BookingId}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Passenger Name:</strong></td>
                        <td style='padding: 8px;'>{await PassengerNames(bookingdata.BookingId)}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Flight:</strong></td>
                        <td style='padding: 8px;'>{flight!.FlightNumber} - {flight.AirlineName}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Departure:</strong></td>
                        <td style='padding: 8px;'>{flight.SourceCity} - {flight.DepartureDateTime}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Arrival:</strong></td>
                        <td style='padding: 8px;'>{flight.DestinationCity} - {flight.ArrivalDateTime}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Cancellation Date:</strong></td>
                        <td style='padding: 8px;'>{DateTime.UtcNow.ToString("MMMM dd, yyyy HH:mm")}</td>
                    </tr>
                   
                </table>

                <p>If you are eligible for a refund, the amount will be credited back to your original payment method within 5-7 business days.</p>

                <p>If you need further assistance, please contact our support team.</p>

                <p><strong>Best Regards,</strong><br>Super Travels Team</p>
            </div>";


            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(fromAddress, appPassword);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;

                await smtp.SendMailAsync(message);
            }
        }
        
}
}