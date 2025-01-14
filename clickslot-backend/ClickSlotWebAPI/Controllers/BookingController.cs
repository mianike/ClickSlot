using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotModel.DTOs;
using ClickSlotModel.Enums;
using ClickSlotWebAPI.Models.Request;
using ClickSlotWebAPI.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickSlotWebAPI.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpGet("master/{masterId}")]
        public async Task<IActionResult> GetBookingsByMasterId(int masterId)
        {
            var bookings = await _bookingService.GetAllByMasterIdAsync(masterId);

            return Ok(_mapper.Map<IEnumerable<BookingResponse>>(bookings));
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var booking = await _bookingService.GetByIdAsync(bookingId);

            return Ok(_mapper.Map<BookingResponse>(booking));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] NewBookingRequest request)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;

            if (currentUser == null)
            {
                return Unauthorized("User is not authorized or not found.");
            }

            var bookingDto = new BookingDTO
            {
                ClientId = currentUser.Id,
                MasterId = request.MasterId,
                OfferingId = request.OfferingId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = BookingStatus.Pending
            };

            var createdBooking = await _bookingService.CreateAsync(bookingDto);

            return CreatedAtAction(nameof(GetBookingById), new { bookingId = createdBooking.Id }, _mapper.Map<BookingResponse>(createdBooking));
        }

        [HttpPut("{bookingId}")]
        public async Task<IActionResult> UpdateBooking(int bookingId, [FromBody] UpdateBookingRequest request)
        {
            var currentUser = HttpContext.Items["CurrentUser"] as AppUserDTO;

            if (currentUser == null)
            {
                return Unauthorized("User is not authorized or not found.");
            }

            var bookingDto = new BookingDTO
            {
                Id = bookingId,
                ClientId = currentUser.Id,
                MasterId = request.MasterId,
                OfferingId = request.OfferingId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = request.Status
            };

            var updatedBooking = await _bookingService.UpdateAsync(bookingDto);

            if (updatedBooking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(_mapper.Map<BookingResponse>(updatedBooking));
        }

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            var result = await _bookingService.DeleteAsync(bookingId);

            if (!result)
            {
                return NotFound("Booking not found.");
            }

            return NoContent();
        }
    }
}
