using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IMasterService : IService
    {
        Task<IEnumerable<AppUserDTO>> GetAllMastersAsync();
        Task<IEnumerable<AppUserDTO>> GetMastersByOfferingNameAsync(string offeringName);
        Task<IEnumerable<AppUserDTO>> GetMastersByNameAsync(string masterName);
    }
}
