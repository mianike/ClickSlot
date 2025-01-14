using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClickSlotCore.Services.Entity
{
    public class ScheduleService : IScheduleService
    {
        private readonly IBookingService _bookingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleService(IBookingService bookingService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _bookingService = bookingService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ScheduleDTO> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Schedule>();

            var schedule = await repository
                .AsReadOnlyQueryable()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (schedule == null)
            {
                throw new KeyNotFoundException($"Schedule with id {id} not found");
            }

            return _mapper.Map<ScheduleDTO>(schedule);
        }
        public async Task<IEnumerable<ScheduleDTO>> GetAllByMasterIdAsync(int masterId)
        {
            var repository = _unitOfWork.GetRepository<Schedule>();

            var schedule = await repository
                .AsReadOnlyQueryable()
                .Where(s => s.MasterId == masterId
                            && s.Date >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(s => s.Date)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ScheduleDTO>>(schedule);
        }

        public async Task<ScheduleDTO> CreateAsync(ScheduleDTO scheduleDto)
        {
            var schedule = _mapper.Map<Schedule>(scheduleDto);

            _unitOfWork.GetRepository<Schedule>().Create(schedule);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ScheduleDTO>(schedule);
        }

        //Сравнение дат нестандартное, но так сравнивает без ошибок
        public async Task<ScheduleDTO> UpdateAsync(ScheduleDTO scheduleDto)
        {
            var existingBookings = await _bookingService
                .GetAllByMasterIdAsync(scheduleDto.MasterId);

            var startTime = scheduleDto.StartTime;
            var endTime = scheduleDto.EndTime;

            //Проверяем, не пересекается ли новое время работы с уже забронированными услугами
            if (!existingBookings.
                Where(b => DateOnly.FromDateTime(b.StartTime.ToLocalTime().Date) == scheduleDto.Date)
                .All(b =>
                    b.StartTime.ToLocalTime() >= scheduleDto.Date
                        .ToDateTime(new TimeOnly(startTime.Hours, startTime.Minutes, startTime.Seconds))
                    && b.EndTime.ToLocalTime() <= scheduleDto.Date
                        .ToDateTime(new TimeOnly(endTime.Hours, endTime.Minutes, endTime.Seconds))))
            {
                throw new InvalidOperationException("Действие запрещено. Имеются записи вне времени начала и окончания");
            }

            var schedule = _mapper.Map<Schedule>(scheduleDto);

            _unitOfWork.GetRepository<Schedule>().Update(schedule);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ScheduleDTO>(schedule);
        }

        //Сравнение дат нестандартное, но так сравнивает без ошибок
        public async Task<bool> DeleteAsync(int id)
        {
            var scheduleDto = await GetByIdAsync(id);
            var existingBookings = await _bookingService
                .GetAllByMasterIdAsync(scheduleDto.MasterId);

            //Проверяем, нет ли на удаляемый день бронирований услуг
            if (existingBookings.Any(b => DateOnly.FromDateTime(b.StartTime.ToLocalTime().Date) == scheduleDto.Date))
            {
                throw new InvalidOperationException("Действие запрещено. На выбранный день имеются записи");
            }



            var schedule = _mapper.Map<Schedule>(scheduleDto);

            var deletedSchedule = _unitOfWork.GetRepository<Schedule>().Delete(schedule);
            await _unitOfWork.SaveChangesAsync();

            return deletedSchedule != null;
        }

        public async Task<ScheduleDTO> GetMasterWorkingDay(int masterId, DateTime dateTime)
        {
            var repository = _unitOfWork.GetRepository<Schedule>();

            var schedule = await repository
                .AsReadOnlyQueryable()
                .FirstOrDefaultAsync(s => s.MasterId == masterId
                                          && s.Date >= DateOnly.FromDateTime(DateTime.Now)
                                          && s.Date == DateOnly.FromDateTime(dateTime));

            return _mapper.Map<ScheduleDTO>(schedule);
        }
    }

}
