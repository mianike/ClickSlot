using ClickSlotModel.DTOs;
using ClickSlotDAL.Entities;
using AutoMapper;
using ClickSlotCore.Contracts.Interfaces;

namespace ClickSlotCore.Mapping
{
    public class ClickSlotProfile : Profile, IService
    {
        public ClickSlotProfile()
        {
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Client, ClientDTO>().ReverseMap();
            CreateMap<Master, MasterDTO>().ReverseMap();
            CreateMap<Offering, OfferingDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Schedule, ScheduleDTO>().ReverseMap();
        }
    }
}