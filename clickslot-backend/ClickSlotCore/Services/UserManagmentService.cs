using ClickSlotCore.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using ClickSlotModel.DTOs;
using ClickSlotCore.Contracts.Interfaces.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

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

        public async Task<string> RegisterAsync(AppUserDTO appUserDto, string password)
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
                throw new AuthenticationException("Error while creating user");
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

            var token = await LoginAsync(appUserDto.Email, password);

            return token;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            if (identityUser == null)
            {
                throw new AuthenticationException("Invalid email");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(identityUser, password);
            if (!passwordValid)
            {
                throw new AuthenticationException("Invalid password.");
            }

            var appUser = await _appUserService.GetByIdentityUserIdAsync(identityUser.Id);
            if (appUser == null)
            {
                throw new AuthenticationException("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            var token = _jwtTokenService.GenerateJwtToken(identityUser, roles, appUser.Id);

            return token;
        }

        public async Task<string> UpdateAsync(AppUserDTO appUserDto)
        {
            var identityUser = await _userManager.FindByEmailAsync(appUserDto.Email);
            if (identityUser == null)
            {
                throw new AuthenticationException("Invalid email");
            }

            var updateUser = await _appUserService.UpdateAsync(appUserDto);
            if (updateUser == null)
            {
                throw new DbUpdateException("Error while updation AppUser");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            var token = _jwtTokenService.GenerateJwtToken(identityUser, roles, appUserDto.Id);
            
            return token;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var appUser = await _appUserService.GetByIdAsync(userId);
            if (appUser == null)
            {
                throw new KeyNotFoundException($"User with id {userId} not found");
            }

            var identityUser = await _userManager.FindByEmailAsync(appUser.Email);
            if (identityUser == null)
            {
                throw new AuthenticationException("Invalid email");
            }

            var result = await _userManager.DeleteAsync(identityUser);
            
            return result.Succeeded;
        }
    }
}
