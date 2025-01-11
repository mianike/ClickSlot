using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IOfferingService : IService
    {
        Task<IEnumerable<OfferingDTO>> GetOfferingsByMasterIdAsync(int masterId);
        Task<OfferingDTO> CreateOfferingAsync(OfferingDTO offeringDto);
        Task<OfferingDTO> UpdateOfferingAsync(OfferingDTO offeringDto);
        Task<bool> DeleteOfferingAsync(int offeringId);
    }
}