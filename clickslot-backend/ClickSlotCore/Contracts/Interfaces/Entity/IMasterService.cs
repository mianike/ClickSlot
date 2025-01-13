using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IMasterService : IService
    {
        Task<IEnumerable<AppUserDTO>> GetFiltredAsync(string search, int page, int pageSize);
    }
}
