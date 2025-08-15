using FlightBooking.Dto;

namespace FlightBooking.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task Register(UserRegisterDto user);
        Task UpdateUserAsync(UserUpdateDto user);
        Task DeleteUserAsync(int id);
        Task<string> Login(UserLoginDto user);

        Task<UserDto> findUserByEmail(string email);

        Task<int> getUsersCount();
        
    }
}