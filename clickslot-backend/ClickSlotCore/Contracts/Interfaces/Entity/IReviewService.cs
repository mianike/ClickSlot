using ClickSlotModel.DTOs;

namespace ClickSlotCore.Contracts.Interfaces.Entity
{
    public interface IReviewService : IService
    {
        Task<ReviewDTO> GetByIdAsync(int id);
        Task<IEnumerable<ReviewDTO>> GetAllByMasterIdAsync(int masterId);
        Task<ReviewDTO> CreateAsync(ReviewDTO reviewDto);
    }
}
