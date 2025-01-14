using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotModel.DTOs;
using ClickSlotWebAPI.Models.Request;
using ClickSlotWebAPI.Models.Response;
using ClickSlotModel.Enums;

namespace ClickSlotWebAPI.Controllers
{
    [ApiController]
    [Route("api/schedules")]
    [Authorize(Roles = "Master")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;

        public ScheduleController(IScheduleService scheduleService, IMapper mapper)
        {
            _scheduleService = scheduleService;
            _mapper = mapper;
        }

        [HttpGet("master/{masterId}")]
        public async Task<IActionResult> GetSchedulesByMasterId(int masterId)
        {
            var schedules = await _scheduleService.GetAllByMasterIdAsync(masterId);

            return Ok(_mapper.Map<IEnumerable<ScheduleResponse>>(schedules));
        }

        [HttpGet("{scheduleId}")]
        public async Task<IActionResult> GetScheduleById(int scheduleId)
        {
            var schedule = await _scheduleService.GetByIdAsync(scheduleId);

            if (schedule == null)
            {
                return NotFound("Schedule not found.");
            }

            return Ok(_mapper.Map<ScheduleResponse>(schedule));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] ScheduleRequest request)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;

            if (currentUser == null)
            {
                return Unauthorized("User is not authorized or not found.");
            }

            if (currentUser.Role != AppUserRole.Master)
            {
                return Forbid("Only masters can create schedules.");
            }

            var scheduleDto = new ScheduleDTO
            {
                MasterId = currentUser.Id,
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            var createdSchedule = await _scheduleService.CreateAsync(scheduleDto);

            return CreatedAtAction(nameof(GetScheduleById), new { scheduleId = createdSchedule.Id }, _mapper.Map<ScheduleResponse>(createdSchedule));
        }

        [HttpPut("{scheduleId}")]
        public async Task<IActionResult> UpdateSchedule(int scheduleId, [FromBody] ScheduleRequest request)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;

            if (currentUser == null)
            {
                return Unauthorized("User is not authorized or not found.");
            }

            if (currentUser.Role != AppUserRole.Master)
            {
                return Forbid("Only masters can update schedules.");
            }

            var scheduleDto = new ScheduleDTO
            {
                Id = scheduleId,
                MasterId = currentUser.Id,
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            var updatedSchedule = await _scheduleService.UpdateAsync(scheduleDto);

            if (updatedSchedule == null)
            {
                return NotFound("Schedule not found.");
            }

            return Ok(_mapper.Map<ScheduleResponse>(updatedSchedule));
        }

        [HttpDelete("{scheduleId}")]
        public async Task<IActionResult> DeleteSchedule(int scheduleId)
        {
            var result = await _scheduleService.DeleteAsync(scheduleId);

            if (!result)
            {
                return NotFound("Schedule not found.");
            }

            return NoContent();
        }
    }
}