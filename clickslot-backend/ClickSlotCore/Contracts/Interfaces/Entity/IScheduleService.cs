using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IScheduleService : IService
    {
        Task<ScheduleDTO> GetByIdAsync(int scheduleId);
        Task<IEnumerable<ScheduleDTO>> GetAllByMasterIdAsync(int masterId);
        Task<ScheduleDTO> CreateAsync(ScheduleDTO scheduleDto);
        Task<ScheduleDTO> UpdateAsync(ScheduleDTO scheduleDto);
        Task<bool> DeleteAsync(int scheduleId);
    }
}