using Microsoft.AspNetCore.Identity;

namespace ClickSlotCore.Contracts.Interfaces
{
    public interface IJwtTokenService : IService
    {
        string GenerateJwtToken(IdentityUser user, IList<string> roles, int appUserId);
    }
}
