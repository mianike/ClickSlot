using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClickSlotCore.Services.Entity
{
    public class OfferingService : IOfferingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OfferingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OfferingDTO>> GetOfferingsByMasterIdAsync(int masterId)
        {
            var repository = _unitOfWork.GetRepository<Offering>();

            var offering = await repository
                .AsReadOnlyQueryable()
                .Where(o => o.MasterId == masterId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OfferingDTO>>(offering);
        }

        public async Task<OfferingDTO> CreateOfferingAsync(OfferingDTO offeringDto)
        {
            var offering = _mapper.Map<Offering>(offeringDto);

            _unitOfWork.GetRepository<Offering>().Create(offering);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OfferingDTO>(offering);
        }

        public async Task<OfferingDTO> UpdateOfferingAsync(OfferingDTO offeringDto)
        {
            var offering = _mapper.Map<Offering>(offeringDto);

            _unitOfWork.GetRepository<Offering>().Update(offering);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OfferingDTO>(offering);
        }

        public async Task<bool> DeleteOfferingAsync(int offeringId)
        {
            var offering = _mapper.Map<Offering>(offeringId);

            var deletedOffering = _unitOfWork.GetRepository<Offering>().Delete(offering);
            await _unitOfWork.SaveChangesAsync();

            return deletedOffering != null;
        }
    }

}
