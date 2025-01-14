using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotCore.Services.Entity;
using ClickSlotModel.DTOs;
using ClickSlotModel.Enums;
using ClickSlotWebAPI.Models.Request;
using ClickSlotWebAPI.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickSlotWebAPI.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetOfferingById(int reviewId)
        {
            var review = await _reviewService.GetByIdAsync(reviewId);

            return Ok(_mapper.Map<OfferingResponse>(review));
        }

        [HttpGet("master/{masterId}")]
        public async Task<IActionResult> GetReviewsByMasterId(int masterId)
        {
            var reviews = await _reviewService.GetAllByMasterIdAsync(masterId);

            return Ok(_mapper.Map<IEnumerable<ReviewResponse>>(reviews));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] ReviewRequest request)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;

            if (currentUser == null)
            {
                return Unauthorized("User is not authorized or not found.");
            }

            var reviewDto = new ReviewDTO()
            {
                ClientId = currentUser.Id,
                MasterId = request.MasterId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            var createdReview = await _reviewService.CreateAsync(reviewDto);

            return CreatedAtAction(nameof(GetOfferingById), new { reviewId = createdReview.Id }, _mapper.Map<ReviewResponse>(createdReview));
        }
    }
}
