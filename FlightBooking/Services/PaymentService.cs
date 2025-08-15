using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlightBooking.Dto;
using FlightBooking.Interface;

namespace FlightBooking.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPayment _repository;
        protected readonly IMapper _mapper;

        public PaymentService(IPayment repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            

        }

        public async Task<IEnumerable<GetPaymentDto>> GetAllPaymentsAsync()
        {
            try
            {
                var payments = await _repository.GetAllAsync();
                var paymentsMap = _mapper.Map<IEnumerable<GetPaymentDto>>(payments);
                return paymentsMap;

            }
            catch(Exception ex)
            {
                 throw new Exception("An error occurred while retrieving Payments.", ex);
            }
        }

        public async Task<GetPaymentDto> GetPaymentByIdAsync(int id)
        {
            var payment = await _repository.GetByIdAsync(id);
            var paymentMap = _mapper.Map<GetPaymentDto>(payment);
            return paymentMap; 
        }

        public async Task<decimal> paymentsTotalSum()
        {
            try
            {
                var totalAmount = await _repository.paymentsSum();
                return totalAmount;
            }
            catch(Exception ex)
            {
                 throw new Exception("An error occurred while retrieving Payments sum.", ex);
            }

        }
    }
}