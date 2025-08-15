using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBooking.Models;

namespace FlightBooking.Interface
{
    public interface IPayment : IGenericRepository<Payment>
    {
        Task<decimal> paymentsSum();
    }
}