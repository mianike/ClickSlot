using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using ClickSlotModel.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClickSlotCore.Services.Entity
{
    public class MasterService : IMasterService
    {
        private readonly IOfferingService _offeringService;
        private readonly IScheduleService _scheduleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MasterService(IOfferingService offeringService, IScheduleService scheduleService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _offeringService = offeringService;
            _scheduleService = scheduleService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUserDTO>> GetFiltredAsync(string search, int page, int pageSize)
        {
            
                var repository = _unitOfWork.GetRepository<AppUser>();

                IQueryable<AppUser> query = repository
                    .AsReadOnlyQueryable()
                    .Where(u => u.Role == AppUserRole.Master && u.Offerings.Any())
                    .Include(o => o.Offerings)
                    .Include(r => r.MasterReviews);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(u => u.Name.ToLower().Contains(search.ToLower())
                    || u.Offerings.Any(o => o.Name.ToLower().Contains(search.ToLower())));
                }

                var appUsers = await query
                    .OrderBy(u => u.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<AppUserDTO>>(appUsers);
        }

        public async Task<AppUserDTO> GetMasterByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<AppUser>();

                var appUser = await repository
                    .AsReadOnlyQueryable()
                    .Include(o => o.Offerings)
                    .Include(s => s.Schedules)
                    .Include(r=>r.MasterReviews
                        .OrderByDescending(b => b.CreatedAt))
                    .ThenInclude(mr=>mr.Client)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (appUser == null || appUser.Role != AppUserRole.Master)
                {
                    throw new KeyNotFoundException($"Master with id {id} not found");
                }

                return _mapper.Map<AppUserDTO>(appUser);
        }

        public async Task<IEnumerable<DateTime>> GetSlotsAsync(int masterId, int offeringId, DateTime date)
        {
            var offering = await _offeringService.GetByIdAsync(offeringId);
            if (offering == null)
            {
                throw new ArgumentException($"Offering with id {offeringId} not found");
            }

            var repository = _unitOfWork.GetRepository<Booking>();

            //При сравнении дат костыль, т.к. скорее всего из-за конвертёра они при обычном Equals отличаются)
            var existingBookings = await repository
                .AsReadOnlyQueryable()
                .Where(b => b.MasterId == masterId
                            && DateOnly.FromDateTime(b.StartTime.Date) == DateOnly.FromDateTime(date.Date))
                .OrderBy(b => b.StartTime)
                .ToListAsync();

            var workingDay = await _scheduleService.GetMasterWorkingDay(masterId, date);

            if (workingDay == null)
            {
                return Enumerable.Empty<DateTime>();
            }

            var availableSlots = new List<DateTime>();
            var slotDuration = offering.Duration;

            var startTime = new DateTime(DateOnly.FromDateTime(date), TimeOnly.FromTimeSpan(workingDay.StartTime));
            var endTime = new DateTime(DateOnly.FromDateTime(date), TimeOnly.FromTimeSpan(workingDay.EndTime));

            while (startTime.Add(slotDuration) <= endTime)
            {
                if (!existingBookings.Any(b =>
                        b.StartTime.ToUniversalTime() < startTime.ToUniversalTime().Add(slotDuration) && b.EndTime.ToUniversalTime() > startTime.ToUniversalTime()))
                {
                    availableSlots.Add(startTime);
                }

                //Шаг между слотами
                startTime = startTime.AddMinutes(15);
            }

            return availableSlots;
        }
    }
}