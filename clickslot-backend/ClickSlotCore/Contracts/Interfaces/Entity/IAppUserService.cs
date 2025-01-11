using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IAppUserService : IService
    {
        Task<IEnumerable<AppUserDTO>> GetAllAsync();
        Task<AppUserDTO> GetByIdAsync(int id);
        Task<AppUserDTO> CreateAsync(AppUserDTO appUserDto);
        Task<AppUserDTO> UpdateAsync(AppUserDTO appUserDto);
        Task<bool> DeleteAsync(AppUserDTO appUserDto);
        Task<AppUserDTO> GetByIdentityUserIdAsync(string id);
    }
}