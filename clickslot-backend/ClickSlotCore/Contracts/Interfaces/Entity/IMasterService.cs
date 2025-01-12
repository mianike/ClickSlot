using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IMasterService : IService
    {
        Task<IEnumerable<AppUserDTO>> GetMastersAsync(string search, int page, int pageSize);
    }
}
