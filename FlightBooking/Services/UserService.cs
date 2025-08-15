using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FlightBooking.Dto;
using FlightBooking.Exceptions;
using FlightBooking.Interface;
using FlightBooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FlightBooking.Services
{
    public class UserService : IUserService
    {

        private readonly IUser _repository;
        protected readonly IMapper _mapper;

        private readonly IConfiguration configuration;


        public UserService(IUser repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            configuration = config;

        }
        public async Task DeleteUserAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _repository.GetAllAsync();
                var usersMap = _mapper.Map<IEnumerable<UserDto>>(users);
                return usersMap;

            }
            catch(Exception ex)
            {
                 throw new Exception("An error occurred while retrieving Users.", ex);
            }

        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
                var user = await _repository.GetByIdAsync(id);
                var userMap = _mapper.Map<UserDto>(user);
                return userMap;
         
        }

        public async Task<string> Login(UserLoginDto user)
        {
            try
            {
                 var userDetails = await _repository.GetUserByEmail(user.Email);

                if(userDetails == null)
                {
                    return "Invalid";
                }

                if(new PasswordHasher<User>().VerifyHashedPassword(userDetails, userDetails.PasswordHash, user.Password)
                    == PasswordVerificationResult.Failed)
                {
                    return "Invalid";

                }
                var  token = CreateToken(userDetails);
                return token;

            }
            catch(Exception ex)
            {
                throw new Exception("Internal Server Error", ex);
            }
           
        }

        public async Task Register(UserRegisterDto user)
        {
            try
            {
                var CheckAccountExists = await _repository.GetUserByEmail(user.Email);

                if(CheckAccountExists != null)
                {
                    throw new AccountAlreadyExistsExcception("Account Already Exists with this email");
                }
                var userMap = _mapper.Map<User>(user);

                var passwordHash = new PasswordHasher<User>()
                    .HashPassword(userMap, user.Password);

                userMap.PasswordHash = passwordHash;
                
                await _repository.AddAsync(userMap);
                await _repository.SuccessfulRegestrationEmail(userMap);
                
            }
            catch(AccountAlreadyExistsExcception)
            {
                throw;
            }
            catch(ArgumentNullException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new Exception("Internal Server Error",ex);
           }
            

       }

        public async Task UpdateUserAsync(UserUpdateDto user)
        {
            try
            {  var findUser = await _repository.GetByIdAsync(user.UserID);

            
                var userMap = _mapper.Map(user, findUser);

                var passwordHash = new PasswordHasher<User>()
                    .HashPassword(userMap!, user.Password);

                userMap!.PasswordHash = passwordHash;
                userMap.CreatedAt = findUser?.CreatedAt;
                userMap.Role = findUser?.Role;

                await _repository.UpdateAsync(userMap);
            }
            catch(KeyNotFoundException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new Exception("Internal Server error!",ex);
            }
        }

        public async Task<UserDto> findUserByEmail(string email)
        {
            try
            {
                var user = await _repository.GetUserByEmail(email);
                if(user != null)
                {
                    var userMap = _mapper.Map<UserDto>(user);
                    return userMap;
                }
                else
                {
                    throw new UserNotFoundException("No user found with this Email");
                }
            }
            catch(UserNotFoundException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new Exception("Error fetching user!", ex);
            }
        }

        //Creating JWT
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role!),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<String>("AppSettings:Token")!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires : DateTime.UtcNow.AddHours(1), 
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        }

        
        public async Task<int> getUsersCount()
        {
            try
            {
                var totalUsers = await _repository.CountAsync();
                return totalUsers;
                
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching the total users.", ex);
            }

        }
    }
}