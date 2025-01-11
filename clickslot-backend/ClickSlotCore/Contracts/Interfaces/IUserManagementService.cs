using ClickSlotModel.DTOs;
using ClickSlotModel.Enums;
using Microsoft.AspNetCore.Identity;

namespace ClickSlotCore.Contracts.Interfaces
{
    public interface IUserManagementService : IService
    {
        Task<IdentityResult> RegisterAsync(AppUserDTO appUser, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
