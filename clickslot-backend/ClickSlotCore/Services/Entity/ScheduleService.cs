using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClickSlotCore.Services.Entity
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
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
                .Where(o => o.MasterId == masterId)
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

        public async Task<ScheduleDTO> UpdateAsync(ScheduleDTO scheduleDto)
        {
            var schedule = _mapper.Map<Schedule>(scheduleDto);

            _unitOfWork.GetRepository<Schedule>().Update(schedule);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ScheduleDTO>(schedule);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var scheduleDto = await GetByIdAsync(id);

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
                .FirstOrDefaultAsync(s => s.MasterId == masterId && s.Date == DateOnly.FromDateTime(dateTime));

            return _mapper.Map<ScheduleDTO>(schedule);
        }
    }

}
