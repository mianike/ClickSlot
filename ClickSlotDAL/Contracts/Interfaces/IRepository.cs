namespace ClickSlotDAL.Contracts.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> AsQueryable();
        IQueryable<TEntity> AsReadOnlyQueryable();
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
    }
}
