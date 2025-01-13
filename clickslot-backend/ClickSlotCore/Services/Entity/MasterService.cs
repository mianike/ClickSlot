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

        public async Task<IEnumerable<AppUserDTO>> GetFiltredAsync(string search, int page, int pageSize)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<AppUser>();

                IQueryable<AppUser> query = repository.AsReadOnlyQueryable()
                    .Where(u => u.Role == AppUserRole.Master && u.Offerings.Any())
                    .Include(u => u.Offerings);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(u => u.Name.ToLower().Contains(search.ToLower())
                    || u.Offerings.Any(o => o.Name.ToLower().Contains(search.ToLower())));
                }

                var appUsers = await query
                    .OrderBy(u => u.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<AppUserDTO>>(appUsers);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error occurred while retrieving masters: {ex.Message}", ex);
            }
        }
    }
}