using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotModel.DTOs;
using ClickSlotModel.Enums;
using ClickSlotWebAPI.Models.Request;

namespace ClickSlotWebAPI.Controllers
{
    [ApiController]
    [Route("api/offerings")]
    public class OfferingController : ControllerBase
    {
        private readonly IOfferingService _offeringService;

        public OfferingController(IOfferingService offeringService)
        {
            _offeringService = offeringService;
        }

        [HttpGet("master/{masterId}")]
        public async Task<IActionResult> GetOfferingsByMasterId(int masterId)
        {
            var offerings = await _offeringService.GetOfferingsByMasterIdAsync(masterId);
            return Ok(offerings);
        }

        [HttpPost]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> CreateOffering([FromBody] OfferingRequest request)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;

            if (currentUser == null)
            {
                return Unauthorized("User is not authorized or not found.");
            }

            if (currentUser.Role != AppUserRole.Master)
            {
                return Forbid("Only masters can create offerings.");
            }

            var offeringDto = new OfferingDTO()
            {
                Name = request.Name,
                Price = request.Price,
                Duration = request.Duration,
                MasterId = currentUser.Id
            };

            var createdOffering = await _offeringService.CreateOfferingAsync(offeringDto);

            return CreatedAtAction(nameof(GetOfferingsByMasterId), new { masterId = createdOffering.MasterId }, createdOffering);
        }

        [HttpDelete("{offeringId}")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> DeleteOffering(int offeringId)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;

            if (currentUser == null)
            {
                return Unauthorized("User is not authorized or not found.");
            }

            if (currentUser.Role != AppUserRole.Master)
            {
                return Forbid("Only masters can delete offerings.");
            }

            var result = await _offeringService.DeleteOfferingAsync(offeringId);

            if (!result)
            {
                return NotFound("Offering not found or you are not authorized to delete it.");
            }

            return NoContent();
        }
    }
}