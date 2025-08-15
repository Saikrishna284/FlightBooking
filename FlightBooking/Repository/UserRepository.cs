using System.Net;
using System.Net.Mail;
using FlightBooking.Data;
using FlightBooking.Interface;
using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.Repository
{
    public class UserRepository : GenericRepository<User>, IUser
    {
        public UserRepository(FlightDbContext context) : base(context){  }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task SuccessfulRegestrationEmail(User user)
        {
            string fromAddress = "saikrishnareddyk28@gmail.com";
            string appPassword = "wuspjkdqbrapahvf";
            string toAddress = user.Email;

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = "Welcome to Safe Travels!",
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(toAddress));

            
           message.Body = $@"
            <div style='font-family: Arial, sans-serif; font-size: 14px; color: #333; line-height: 1.6; max-width: 500px;'>
                <p>Dear <strong>{user.Name}</strong>,</p>

                <p>Welcome to <strong>Super Travels</strong>! We are excited to have you on board.</p>

                <table style='width: 100%; border-collapse: collapse;'>
                    <tr>
                        <td style='padding: 8px;'><strong>Registered Email:</strong></td>
                        <td style='padding: 8px;'>{user.Email}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Registration Date:</strong></td>
                        <td style='padding: 8px;'>{DateTime.UtcNow.ToString("MMMM dd, yyyy")}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px;'><strong>Username:</strong></td>
                        <td style='padding: 8px;'>{user.Name}</td>
                    </tr>
                </table>

                <p>Your account has been successfully created. You can now log in and start booking flights hassle-free.</p>

                <p>For any assistance, feel free to contact our support team.</p>

                <p><strong>Happy Travels!</strong><br>Super Travels Team</p>
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