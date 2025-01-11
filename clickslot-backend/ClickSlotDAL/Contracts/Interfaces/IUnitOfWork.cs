using Microsoft.EntityFrameworkCore.Storage;

namespace ClickSlotDAL.Contracts.Interfaces
{
    public interface IUnitOfWork
    {
        IDbContextTransaction BeginTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;

        Task<int> SaveChangesAsync();
    }
}
