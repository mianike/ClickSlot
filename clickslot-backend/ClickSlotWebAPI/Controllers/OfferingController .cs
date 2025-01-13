using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotModel.DTOs;
using ClickSlotModel.Enums;
using ClickSlotWebAPI.Models.Request;
using ClickSlotWebAPI.Models.Response;
using ClickSlotDAL.Entities;

namespace ClickSlotWebAPI.Controllers
{
    [ApiController]
    [Route("api/offerings")]
    [Authorize(Roles = "Master")]
    public class OfferingController : ControllerBase
    {
        private readonly IOfferingService _offeringService;
        private readonly IMapper _mapper;

        public OfferingController(IOfferingService offeringService, IMapper mapper)
        {
            _offeringService = offeringService;
            _mapper = mapper;
        }

        [HttpGet("master/{masterId}")]
        public async Task<IActionResult> GetOfferingsByMasterId(int masterId)
        {
            var offerings = await _offeringService.GetAllByMasterIdAsync(masterId);
            
            return Ok(_mapper.Map<IEnumerable<OfferingResponse>>(offerings));
        }

        [HttpGet("{offeringId}")]
        public async Task<IActionResult> GetOfferingById(int offeringId)
        {
            var offering = await _offeringService.GetByIdAsync(offeringId);
            
            return Ok(_mapper.Map<OfferingResponse>(offering));
        }

        [HttpPost]
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

            var createdOffering = await _offeringService.CreateAsync(offeringDto);

            return CreatedAtAction(nameof(GetOfferingsByMasterId), new { masterId = createdOffering.MasterId }, createdOffering);
        }

        [HttpPut("{offeringId}")]
        public async Task<IActionResult> UpdateOffering(int offeringId, [FromBody] OfferingRequest request)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;

            if (currentUser == null)
            {
                return Unauthorized("User is not authorized or not found.");
            }

            if (currentUser.Role != AppUserRole.Master)
            {
                return Forbid("Only masters can update offerings.");
            }

            var offeringDto = new OfferingDTO()
            {
                Id = offeringId,
                Name = request.Name,
                Price = request.Price,
                Duration = request.Duration,
                MasterId = currentUser.Id
            };

            var updatedOffering = await _offeringService.UpdateAsync(offeringDto);

            if (updatedOffering == null)
            {
                return NotFound("Offering not found.");
            }

            return Ok(_mapper.Map<OfferingResponse>(updatedOffering));
        }


        [HttpDelete("{offeringId}")]
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

            var result = await _offeringService.DeleteAsync(offeringId);

            if (!result)
            {
                return NotFound("Offering not found or you are not authorized to delete it.");
            }

            return NoContent();
        }
    }
}