using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using ClickSlotModel.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClickSlotCore.Services.Entity
{
    public class MasterService : IMasterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MasterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUserDTO>> GetAllMastersAsync()
        {
            try
            {
                var repository = _unitOfWork.GetRepository<AppUser>();

                var appUsers = await repository
                    .AsReadOnlyQueryable()
                    .Where(u => u.Role == AppUserRole.Master)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<AppUserDTO>>(appUsers);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving masters: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AppUserDTO>> GetMastersByOfferingNameAsync(string offeringName)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<AppUser>();

                var appUsers = await repository
                    .AsReadOnlyQueryable()
                    .Where(u => u.Role == AppUserRole.Master && u.Offerings
                        .Any(o => o.Name.
                            ToLower()
                            .Contains(offeringName.ToLower())))
                    .Include(u => u.Offerings)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<AppUserDTO>>(appUsers);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving masters by offering name '{offeringName}': {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AppUserDTO>> GetMastersByNameAsync(string masterName)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<AppUser>();

                var appUsers = await repository
                    .AsReadOnlyQueryable()
                    .Where(u => u.Role == AppUserRole.Master && u.Name
                        .ToLower()
                        .Contains(masterName.ToLower()))
                    .ToListAsync();

                return _mapper.Map<IEnumerable<AppUserDTO>>(appUsers);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving masters by name '{masterName}': {ex.Message}", ex);
            }
        }

    }
}
