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
            CreateMap<OfferingDTO, OfferingResponse>();
            CreateMap<ScheduleDTO, ScheduleResponse>();

            CreateMap<AppUserDTO, MasterResponse>().
                ForMember(dest => dest.OfferingsCount,
                    opt => opt
                        .MapFrom(src => src.Offerings.Count))
                .ForMember(dest => dest.Rating,
                    opt => opt.MapFrom(src => src.MasterReviews != null && src.MasterReviews.Any() ? Math.Round(src.MasterReviews.Select(r => (double)r.Rating).Average(), 2) : 0))
                .ForMember(dest => dest.ReviewsCount,
                    opt => opt.MapFrom(src => src.MasterReviews != null ? src.MasterReviews.Count() : 0));
            
            CreateMap<AppUserDTO, SingleMasterResponse>()
                .ForMember(dest => dest.Rating,
                    opt => opt.MapFrom(src => src.MasterReviews != null && src.MasterReviews.Any() ? Math.Round(src.MasterReviews.Select(r => (double)r.Rating).Average(),2) : 0))
                .ForMember(dest => dest.ReviewsCount,
                    opt => opt.MapFrom(src => src.MasterReviews != null ? src.MasterReviews.Count() : 0));

            CreateMap<BookingDTO, BookingResponse>()
                .ForMember(dest => dest.ClientName,
                    opt => opt
                        .MapFrom(src => src.Client.Name))
                .ForMember(dest => dest.ClientPhone,
                    opt => opt
                        .MapFrom(src => src.Client.Phone))
                .ForMember(dest => dest.OfferingName,
                    opt => opt
                        .MapFrom(src => src.Offering.Name));

            CreateMap<ReviewDTO, ReviewResponse>()
                .ForMember(dest => dest.ClientName,
                    opt => opt
                        .MapFrom(src => src.Client.Name));
        }
    }
}