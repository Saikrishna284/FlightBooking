using AutoMapper;
using FlightBooking.Dto;
using FlightBooking.Models;

namespace FlightBooking.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Flight, FlightDto>().ReverseMap();
            CreateMap<AddFlightDto, Flight>();
            CreateMap<UpdateFlightDto, Flight>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<BookingDto, Booking>().ReverseMap();
            CreateMap<GetBookingDto, Booking>().ReverseMap();
            CreateMap<PassengerDto, Passenger>().ReverseMap();
            CreateMap<Passenger, GetPassengerDto>();
            CreateMap<PaymentDto, Payment>().ReverseMap();
            CreateMap<Payment, GetPaymentDto>();
            CreateMap<CheckIn, GetCheckinDto>();
            CreateMap<CheckinDto, CheckIn>();
            CreateMap<Booking, BookingInfoDto>();
            

           
        }
    }
}