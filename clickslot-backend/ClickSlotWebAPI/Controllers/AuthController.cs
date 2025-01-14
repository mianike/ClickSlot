using System.Security.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ClickSlotCore.Contracts.Interfaces;
using ClickSlotModel.DTOs;
using ClickSlotWebAPI.Models.Request;
using ClickSlotWebAPI.Models.Response;
using Microsoft.AspNetCore.Authorization;

namespace ClickSlotWebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IMapper _mapper;

        public AuthController(IUserManagementService userManagementService, IMapper mapper)
        {
            _userManagementService = userManagementService;
            _mapper = mapper;
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;
            if (currentUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AppUserResponse>(currentUser));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var appUserDto = new AppUserDTO()
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                Role = request.Role
            };

            try
            {
                var token = await _userManagementService.RegisterAsync(appUserDto, request.Password);
                return Ok(new { Token = token });
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _userManagementService.LoginAsync(request.Email, request.Password);
                return Ok(new { Token = token });
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateAppUserRequest request)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;
            if (currentUser == null)
            {
                return NotFound();
            }

            var appUserDto = new AppUserDTO
            {
                Id = currentUser.Id,
                Email = currentUser.Email,

                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
            };

            try
            {
                var token = await _userManagementService.UpdateAsync(appUserDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new { message = "Updating error", details = ex.Message }
                });
            }
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _userManagementService.DeleteAsync(userId);
                    return result? NoContent(): BadRequest("User not deleted!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = new { message = "Deleting error", details = ex.Message }
                });
            }
        }
    }
}