using FlightBooking.Dto;

namespace FlightBooking.Interface
{
    public interface ICheckinService
    {
        Task Checkin(CheckinDto data);

        Task<IEnumerable<GetCheckinDto>> GetAllCheckinsAsync();
        Task<GetCheckinDto> GetCheckinByIdAsync(int id);
    }
}