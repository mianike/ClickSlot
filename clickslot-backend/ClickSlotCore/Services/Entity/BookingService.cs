using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClickSlotCore.Services.Entity
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookingDTO> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Booking>();

            var booking = await repository
                .AsReadOnlyQueryable()
                .Include(c => c.Client)
                .Include(o => o.Offering)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with id {id} not found");
            }

            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task<IEnumerable<BookingDTO>> GetAllByMasterIdAsync(int masterId)
        {
            var repository = _unitOfWork.GetRepository<Booking>();

            var bookings = await repository
                .AsReadOnlyQueryable()
                .Where(b => b.MasterId == masterId
                            && b.StartTime.ToLocalTime() >= DateTime.Now)
                .Include(c => c.Client)
                .Include(o => o.Offering)
                .OrderBy(b => b.StartTime)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
        }

        public async Task<IEnumerable<BookingDTO>> GetAllByMasterIdAsync(int masterId, int page, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<Booking>();

            var bookings = await repository
                .AsReadOnlyQueryable()
                .Where(b => b.MasterId == masterId
                            && b.StartTime.ToLocalTime() >= DateTime.Now)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.Client)
                .Include(o => o.Offering)
                .OrderBy(b => b.StartTime)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
        }

        public async Task<BookingDTO> CreateAsync(BookingDTO bookingDto)
        {
            var booking = _mapper.Map<Booking>(bookingDto);

            _unitOfWork.GetRepository<Booking>().Create(booking);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task<BookingDTO> UpdateAsync(BookingDTO bookingDto)
        {
            var booking = _mapper.Map<Booking>(bookingDto);

            _unitOfWork.GetRepository<Booking>().Update(booking);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookingDTO>(booking);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bookingDto = await GetByIdAsync(id);

            var booking = _mapper.Map<Booking>(bookingDto);

            var deletedBooking = _unitOfWork.GetRepository<Booking>().Delete(booking);
            await _unitOfWork.SaveChangesAsync();

            return deletedBooking != null;
        }

    }
}