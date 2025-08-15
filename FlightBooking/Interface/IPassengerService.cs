using FlightBooking.Dto;


namespace FlightBooking.Interface
{
    public interface IPassengerService
    {
        Task<IEnumerable<GetPassengerDto>> GetAllPassengers();
        Task<GetPassengerDto> GetPassengerByIdAsync(int id);

        Task<IEnumerable<GetPassengerDto>> GetPassengersByFlightIdAsync(int flightId);
        
       
    }
}