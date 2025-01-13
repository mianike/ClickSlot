using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
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
            var masterResponses = _mapper.Map<List<AppUserResponse>>(masterDtos);
            return Ok(masterResponses);
        }
    }
}