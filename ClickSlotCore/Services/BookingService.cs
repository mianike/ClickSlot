using AutoMapper;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;

namespace ClickSlotCore.Services
{
    public class BookingService : EntityService<BookingDTO, Booking>
    {
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }
    }
}