using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlightBooking.Dto;
using FlightBooking.Interface;
using FlightBooking.Models;

namespace FlightBooking.Services
{
    public class CheckinService : ICheckinService
    {
        private readonly ICheckin _repository;
        private readonly IMapper _mapper;

        public CheckinService(ICheckin repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Checkin(CheckinDto data)
        {
            if(await _repository.passengerVerification(data.PassengerId))
            {
                var checkinMap  = _mapper.Map<CheckIn>(data);
                checkinMap.CheckInDate = DateTime.Now;
                await _repository.AddAsync(checkinMap);

            }
           
        }

        public async Task<IEnumerable<GetCheckinDto>> GetAllCheckinsAsync()
        {
           try
            {
                var checkins = await _repository.GetAllAsync();
                var checkinsMap = _mapper.Map<IEnumerable<GetCheckinDto>>(checkins);
                return checkinsMap;

            }
            catch(Exception ex)
            {
                 throw new Exception("An error occurred while retrieving Checkins.", ex);
            }
        }

        public async Task<GetCheckinDto> GetCheckinByIdAsync(int id)
        {
            var checkin = await _repository.GetByIdAsync(id);
            var checkinMap = _mapper.Map<GetCheckinDto>(checkin);
            return checkinMap; 
            
        }

    }
}