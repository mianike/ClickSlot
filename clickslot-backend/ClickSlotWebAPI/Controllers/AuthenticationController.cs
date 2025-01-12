using Microsoft.AspNetCore.Mvc;
using ClickSlotCore.Contracts.Interfaces;
using ClickSlotModel.DTOs;
using ClickSlotWebAPI.Models.Request;
using Microsoft.AspNetCore.Authorization;

namespace ClickSlotWebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public AuthenticationController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
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

            var result = await _userManagementService.RegisterAsync(appUserDto, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                var token = await _userManagementService.LoginAsync(request.Email, request.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
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
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
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
            return Ok(currentUser);
        }
    }
}