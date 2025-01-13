using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IOfferingService : IService
    {
        Task<OfferingDTO> GetByIdAsync(int offeringId);
        Task<IEnumerable<OfferingDTO>> GetAllByMasterIdAsync(int masterId);
        Task<OfferingDTO> CreateAsync(OfferingDTO offeringDto);
        Task<OfferingDTO> UpdateAsync(OfferingDTO offeringDto);
        Task<bool> DeleteAsync(int offeringId);
    }
}