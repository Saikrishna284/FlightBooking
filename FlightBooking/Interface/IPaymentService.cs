
using FlightBooking.Dto;

namespace FlightBooking.Interface
{
    public interface IPaymentService
    {
        Task<IEnumerable<GetPaymentDto>> GetAllPaymentsAsync();
        Task<GetPaymentDto> GetPaymentByIdAsync(int id);

        Task<decimal> paymentsTotalSum();
        
    }
}