using AutoMapper;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;

namespace ClickSlotCore.Services
{
    public class OfferingService : EntityService<OfferingDTO, Offering>
    {
        public OfferingService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }
    }
}
