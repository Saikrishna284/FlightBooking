using FlightBooking.Dto;

namespace FlightBooking.Interface
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightDto>> GetAllFlightsAsync();
        Task<FlightDto> GetFlightByIdAsync(int id);
        Task AddFlightAsync(AddFlightDto flight);
        Task UpdateFlightAsync(UpdateFlightDto flight);
        Task DeleteFlightAsync(int id);
        Task<IEnumerable<FlightDto>> SearchFlights(string source, string destination, DateTime date);

        Task<int> getFlightsCount();

    }
}