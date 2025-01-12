using ClickSlotModel.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ClickSlotCore.Contracts.Interfaces
{
    public interface IUserManagementService : IService
    {
        Task<string> RegisterAsync(AppUserDTO appUser, string password);
        Task<string> LoginAsync(string email, string password);
        Task<string> UpdateAsync(AppUserDTO appUser);
        Task<bool> DeleteAsync(int userId);
    }
}
