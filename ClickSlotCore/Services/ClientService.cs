using AutoMapper;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;

namespace ClickSlotCore.Services
{
    public class ClientService : EntityService<ClientDTO, Client>
    {
        public ClientService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }
    }
}
