using ClickSlotDAL.Contracts.Interfaces;

namespace ClickSlotCore.Contracts.Interfaces
{
    public interface IEntityService<TDTO, TEntity> where TDTO : class where TEntity : class, IEntity
    {
        Task<IEnumerable<TDTO>> GetAllAsync();
        Task<TDTO> GetByIdAsync(int id);
        Task<TDTO> CreateAsync(TDTO dto);
        Task<TDTO> UpdateAsync(TDTO dto);
        Task<bool> DeleteAsync(TDTO dto);
    }
}
