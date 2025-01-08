using ClickSlotDAL.Contexts;
using ClickSlotDAL.Contracts.Exceptions;
using ClickSlotDAL.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace ClickSlotDAL.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClickSlotDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(ClickSlotDbContext context)
        {
            _context = context;
        }

        public IDbContextTransaction BeginTransaction()
        {
            lock (_context)
            {
                if (_currentTransaction != null)
                {
                    throw new UnitOfWorkAlreadyInTransactionStateException();
                }

                _currentTransaction = _context.Database.BeginTransaction();
            }
            return _currentTransaction;
        }

        IRepository<TEntity> IUnitOfWork.GetRepository<TEntity>()
        {
            return new Repository<TEntity>(_context.Set<TEntity>());
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
