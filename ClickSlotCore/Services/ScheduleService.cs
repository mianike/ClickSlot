using AutoMapper;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;

namespace ClickSlotCore.Services
{
    public class ScheduleService : EntityService<ScheduleDTO, Schedule>
    {
        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }
    }
}
