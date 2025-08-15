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
    public class FlightService : IFlightService
    {
        private readonly IFlight _repository;
        protected readonly IMapper _mapper;

        public FlightService(IFlight repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightDto>> GetAllFlightsAsync()
        {
            try
            {
                var flights = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<FlightDto>>(flights);
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching the flights.", ex);
            }
           
        }

        public async Task<FlightDto> GetFlightByIdAsync(int id)
        {
           
           var flight = await _repository.GetByIdAsync(id);
            return _mapper.Map<FlightDto>(flight);
         
        }

        public async Task AddFlightAsync(AddFlightDto flight)
        {
            try
            {
                var  flightMap = _mapper.Map<Flight>(flight);
                await _repository.AddAsync(flightMap);
            }
            catch (ArgumentNullException)
            {
                throw ;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding entity in service layer.", ex);
            }

        }

        public async Task UpdateFlightAsync(UpdateFlightDto flight)
        {
            try
            {
                var findFlight = await _repository.GetByIdAsync(flight.FlightID);
                var flightMap = _mapper.Map(flight, findFlight);
                flightMap!.FlightNumber = findFlight?.FlightNumber;
                flightMap.AirlineName = findFlight?.AirlineName;

                await _repository.UpdateAsync(flightMap);
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

        public async Task DeleteFlightAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<FlightDto>> SearchFlights(string source, string destination, DateTime date)
        {
            try
            {
                var flights = await _repository.SearchFlights(source, destination, date);

                if (!flights.Any())
                {
                    throw new KeyNotFoundException("No flights found for the selected route.");
                }
                if(date.Date == DateTime.UtcNow.Date)
                {
                    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

                    flights = flights.Where(f => f.DepartureDateTime >= indianTime).ToList();
                }
                var flightsMap = _mapper.Map<IEnumerable<FlightDto>>(flights);
            
                return flightsMap;
            }
            catch(KeyNotFoundException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while processing the flight search.", ex);
            }
           
        }

        public async Task<int> getFlightsCount()
        {
            try
            {
                var totalFlights = await _repository.CountAsync();
                return totalFlights;
                
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while fetching the total flights.", ex);
            }

        }
    }
}