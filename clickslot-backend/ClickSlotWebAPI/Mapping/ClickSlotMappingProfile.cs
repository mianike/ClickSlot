using AutoMapper;
using ClickSlotModel.DTOs;
using ClickSlotDAL.Entities;
using ClickSlotWebAPI.Models.Response;

namespace ClickSlotWebAPI.Mapping
{
    public class ClickSlotMappingProfile : Profile
    {
        public ClickSlotMappingProfile()
        {
            CreateMap<AppUser, AppUserDTO>().ReverseMap();
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Offering, OfferingDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Schedule, ScheduleDTO>().ReverseMap();

            CreateMap<AppUserDTO, AppUserResponse>();
            CreateMap<AppUserDTO, SingleMasterResponse>();
            CreateMap<OfferingDTO, OfferingResponse>();
            CreateMap<ScheduleDTO, ScheduleResponse>();
        }
    }
}