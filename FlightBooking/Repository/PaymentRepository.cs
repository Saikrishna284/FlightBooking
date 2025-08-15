using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBooking.Data;
using FlightBooking.Interface;
using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPayment
    {
        public PaymentRepository(FlightDbContext context) : base (context) { }

        public async Task<decimal> paymentsSum()
        {
            var totalAmount = await _context.Payments.Where(p => p.PaymentStatus == "Paid").SumAsync(p => p.AmountPaid);
            return totalAmount;
        }
    }
}