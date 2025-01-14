using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IScheduleService : IService
    {
        Task<ScheduleDTO> GetByIdAsync(int id);
        Task<IEnumerable<ScheduleDTO>> GetAllByMasterIdAsync(int masterId);
        Task<ScheduleDTO> CreateAsync(ScheduleDTO scheduleDto);
        Task<ScheduleDTO> UpdateAsync(ScheduleDTO scheduleDto);
        Task<bool> DeleteAsync(int id);
        Task<ScheduleDTO> GetMasterWorkingDay(int masterId, DateTime dateTime);
    }
}