using AutoMapper;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;

namespace ClickSlotCore.Services
{
    public class MasterService : EntityService<MasterDTO, Master>
    {
        public MasterService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }
    }
}
