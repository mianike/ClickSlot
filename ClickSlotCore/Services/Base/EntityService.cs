using AutoMapper;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotCore.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClickSlotCore.Services
{
    public class EntityService<TDTO, TEntity> : IEntityService<TDTO, TEntity> where TDTO : class where TEntity : class, IEntity
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDTO>> GetAllAsync()
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TEntity>();

                var entities = await repository
                    .AsReadOnlyQueryable()
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TDTO>>(entities);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                //_logger.LogError(ex, $"{typeof(TEntity)}. Error occurred while retrieving entities");

                throw new ApplicationException($"{typeof(TEntity)}. Error occurred while retrieving entities: {ex.Message}", ex);
            }
        }

        public async Task<TDTO> GetByIdAsync(int id)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TEntity>();

                var entity = await repository
                    .AsReadOnlyQueryable()
                    .FirstOrDefaultAsync(p => EF.Property<int>(p, "Id") == id);

                if (entity == null)
                {
                    throw new KeyNotFoundException($"{typeof(TEntity)} with id {id} not found");
                }

                return _mapper.Map<TDTO>(entity);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                //_logger.LogError(ex, $"{typeof(TEntity)}. Error occurred while retrieving entity with id {id}");

                throw new ApplicationException($"{typeof(TEntity)}. Error occurred while retrieving entity with id {id}: {ex.Message}", ex);
            }
        }

        public async Task<TDTO> CreateAsync(TDTO dto)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(dto);

                _unitOfWork.GetRepository<TEntity>().Create(entity);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<TDTO>(entity);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                // _logger.LogError(ex, $"{typeof(TEntity)}. Error occurred while creating entity");

                throw new ApplicationException($"Error occurred while creating entity of type {typeof(TEntity)}: {ex.Message}", ex);
            }
        }

        public async Task<TDTO> UpdateAsync(TDTO dto)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(dto);

                _unitOfWork.GetRepository<TEntity>().Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<TDTO>(entity);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                // _logger.LogError(ex, $"{typeof(TEntity)}. Error occurred while updating entity");

                throw new ApplicationException($"Error occurred while updating entity of type {typeof(TEntity)}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(TDTO dto)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(dto);

                var deletedEntity = _unitOfWork.GetRepository<TEntity>().Delete(entity);
                await _unitOfWork.SaveChangesAsync();

                return deletedEntity != null;
            }
            catch (Exception ex)
            {
                //TODO: add logger
                // _logger.LogError(ex, $"{typeof(TEntity)}. Error occurred while deleting entity");

                throw new ApplicationException($"Error occurred while deleting entity of type {typeof(TEntity)}: {ex.Message}", ex);
            }
        }
    }
}
