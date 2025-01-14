using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using ClickSlotWebAPI.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ClickSlotWebAPI.Controllers
{
    [ApiController]
    [Route("api/masters")]
    public class MasterController : ControllerBase
    {
        private readonly IMasterService _masterService;
        private readonly IMapper _mapper;

        public MasterController(IMasterService masterService, IMapper mapper)
        {
            _masterService = masterService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMasters(
            string? search = null,
            int page = 1,
            int pageSize = 10)
        {
            var masterDtos = await _masterService.GetFiltredAsync(search, page, pageSize);
            
            return Ok(_mapper.Map<IEnumerable<MasterResponse>>(masterDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMasterById(int id)
        {
            var masterDto = await _masterService.GetMasterByIdAsync(id);
            if (masterDto == null)
            {
                return NotFound("Мастер не найден");
            }

            var masterResponse = _mapper.Map<SingleMasterResponse>(masterDto);

            return Ok(masterResponse);
        }

        [HttpGet("{masterId}/offerings/{offeringId}/slots")]
        public async Task<IActionResult> GetSlots(int masterId, int offeringId, DateTime date)
        {
            try
            {
                var slots = await _masterService.GetSlotsAsync(masterId, offeringId, date);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}