using Microsoft.EntityFrameworkCore;
using ClickSlotDAL.Contracts.Interfaces;

namespace ClickSlotDAL.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbSet<TEntity> dbSet)
        {
            _dbSet = dbSet;
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TEntity> AsReadOnlyQueryable()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public TEntity Create(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public TEntity Update(TEntity entity)
        {
            return _dbSet.Update(entity).Entity;
        }

        public TEntity Delete(TEntity entity)
        {
            return _dbSet.Remove(entity).Entity;
        }
    }
}
