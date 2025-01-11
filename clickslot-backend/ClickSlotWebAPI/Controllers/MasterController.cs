using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotWebAPI.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ClickSlotWebAPI.Models.Response.AppUserResponse;

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
        public async Task<IActionResult> GetAllMasters()
        {
            var masterDtos = await _masterService.GetAllMastersAsync();
            var masterResponses = _mapper.Map<List<AppUserResponse>>(masterDtos);

            return Ok(masterResponses);
        }

        [HttpGet("offering/{offeringName}")]
        public async Task<IActionResult> GetMastersByOfferingName(string offeringName)
        {
            var masterDtos = await _masterService.GetMastersByOfferingNameAsync(offeringName);
            var masterResponses = _mapper.Map<List<AppUserResponse>>(masterDtos);
            return Ok(masterResponses);
        }

        [HttpGet("name/{masterName}")]
        public async Task<IActionResult> GetMastersByName(string masterName)
        {
            var masterDtos = await _masterService.GetMastersByNameAsync(masterName);
            var masterResponses = _mapper.Map<List<AppUserResponse>>(masterDtos);
            return Ok(masterResponses);
        }
    }
}