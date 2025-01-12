using AutoMapper;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using Microsoft.EntityFrameworkCore;
using ClickSlotCore.Contracts.Interfaces.Entity;

namespace ClickSlotCore.Services.Entity
{
    public class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppUserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUserDTO>> GetAllAsync()
        {
            try
            {
                var repository = _unitOfWork.GetRepository<AppUser>();

                var appUsers = await repository
                    .AsReadOnlyQueryable()
                    .ToListAsync();

                return _mapper.Map<IEnumerable<AppUserDTO>>(appUsers);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                //_logger.LogError(ex, $"Error occurred while retrieving AppUsers");

                throw new ApplicationException($"Error occurred while retrieving entities: {ex.Message}", ex);
            }
        }

        public async Task<AppUserDTO> GetByIdAsync(int id)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<AppUser>();

                var appUser = await repository
                    .AsReadOnlyQueryable()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (appUser == null)
                {
                    throw new KeyNotFoundException($"AppUser with id {id} not found");
                }

                return _mapper.Map<AppUserDTO>(appUser);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                //_logger.LogError(ex, $"Error occurred while retrieving AppUser with id {id}");

                throw new ApplicationException($"Error occurred while retrieving AppUser with id {id}: {ex.Message}", ex);
            }
        }

        public async Task<AppUserDTO> CreateAsync(AppUserDTO appUserDto)
        {
            try
            {
                var appUser = _mapper.Map<AppUser>(appUserDto);

                _unitOfWork.GetRepository<AppUser>().Create(appUser);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<AppUserDTO>(appUser);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                // _logger.LogError(ex, $"Error occurred while creating AppUser");

                throw new ApplicationException($"Error occurred while creating AppUser : {ex.Message}", ex);
            }
        }

        public async Task<AppUserDTO> UpdateAsync(AppUserDTO appUserDto)
        {
            try
            {
                var existAppUserDto = await GetByIdAsync(appUserDto.Id);
                
                var appUser = _mapper.Map<AppUser>(existAppUserDto);

                appUser.Name = appUserDto.Name;
                appUser.Phone = appUserDto.Phone;
                appUser.Address = appUserDto.Address;

                _unitOfWork.GetRepository<AppUser>().Update(appUser);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<AppUserDTO>(appUser);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                // _logger.LogError(ex, $"Error occurred while updating AppUser");

                throw new ApplicationException($"Error occurred while updating AppUser: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(AppUserDTO appUserDto)
        {
            try
            {
                var appUser = _mapper.Map<AppUser>(appUserDto);

                var deletedAppUsers = _unitOfWork.GetRepository<AppUser>().Delete(appUser);
                await _unitOfWork.SaveChangesAsync();

                return deletedAppUsers != null;
            }
            catch (Exception ex)
            {
                //TODO: add logger
                // _logger.LogError(ex, $"Error occurred while deleting AppUser");

                throw new ApplicationException($"Error occurred while deleting AppUser: {ex.Message}", ex);
            }
        }

        public async Task<AppUserDTO> GetByIdentityUserIdAsync(string id)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<AppUser>();

                var appUsers = await repository
                    .AsReadOnlyQueryable()
                    .FirstOrDefaultAsync(u => u.IdentityUserId == id);

                if (appUsers == null)
                {
                    throw new KeyNotFoundException($"AppUser with IdentityUserId {id} not found");
                }

                return _mapper.Map<AppUserDTO>(appUsers);
            }
            catch (Exception ex)
            {
                //TODO: add logger
                //_logger.LogError(ex, $"Error occurred while retrieving AppUser with id {id}");

                throw new ApplicationException($"Error occurred while retrieving AppUser with id {id}: {ex.Message}", ex);
            }
        }
    }
}
