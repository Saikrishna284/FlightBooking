using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AutoMapper;
using FlightBooking.Dto;
using FlightBooking.Interface;
using FlightBooking.Models;

namespace FlightBooking.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassenger _repository;

        private readonly IMapper _mapper;

        public PassengerService(IPassenger repository, IMapper mapper)
        {
            _repository = repository;
            _mapper  = mapper;
        }

        public async Task<IEnumerable<GetPassengerDto>> GetAllPassengers()
        {
            try
            {
                var passengers =  await _repository.GetAllAsync();
                var passengerMap = _mapper.Map<IEnumerable<GetPassengerDto>>(passengers);
                return passengerMap;

            }
            catch(Exception ex)
            {
                 throw new Exception("An error occurred while retrieving Passengers.", ex);
            }
            
        }

        public async Task<GetPassengerDto> GetPassengerByIdAsync(int id)
        {
           
            var passenger = await _repository.GetByIdAsync(id);
            var passengerMap = _mapper.Map<GetPassengerDto>(passenger);
            return passengerMap; 
          
            
        }

        public async Task<IEnumerable<GetPassengerDto>> GetPassengersByFlightIdAsync(int flightId)
        {
            try
            {
                var passengers = await _repository.GetPassengersByFlightId(flightId);
                var passengersMap = _mapper.Map<IEnumerable<GetPassengerDto>>(passengers);
                return passengersMap;

            }
            catch(Exception ex)
            {
                 throw new Exception("An error occurred while retrieving Passengers.", ex);
            }
          
        }
    }
}