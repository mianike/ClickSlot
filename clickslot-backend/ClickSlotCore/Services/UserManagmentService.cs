using ClickSlotCore.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using ClickSlotModel.DTOs;
using ClickSlotModel.Enums;
using ClickSlotCore.Contracts.Interfaces.Entity;

namespace ClickSlotCore.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IAppUserService _appUserService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;

        public UserManagementService(
            IAppUserService appUserService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenService jwtTokenService)
        {
            _appUserService = appUserService;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<IdentityResult> RegisterAsync(AppUserDTO appUserDto, string password)
        {
            var identityUser = new IdentityUser
            {
                UserName = appUserDto.Email,
                Email = appUserDto.Email,
                PhoneNumber = appUserDto.Phone
            };

            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
            {
                return result;
            }

            appUserDto.IdentityUserId = identityUser.Id;
            appUserDto.CreatedAt = DateTime.UtcNow;

            await _appUserService.CreateAsync(appUserDto);

            var roleName = appUserDto.Role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(identityUser, roleName);

            return IdentityResult.Success;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            if (identityUser == null)
            {
                throw new UnauthorizedAccessException("Invalid email");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(identityUser, password);
            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid password.");
            }

            var appUser = await _appUserService.GetByIdentityUserIdAsync(identityUser.Id);
            if (appUser == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            var token = _jwtTokenService.GenerateJwtToken(identityUser, roles, appUser.Id);

            return token;
        }
    }
}
