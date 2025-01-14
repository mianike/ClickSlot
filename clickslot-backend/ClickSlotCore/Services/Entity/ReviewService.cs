using AutoMapper;
using ClickSlotCore.Contracts.Interfaces.Entity;
using ClickSlotDAL.Contracts.Interfaces;
using ClickSlotDAL.Entities;
using ClickSlotModel.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClickSlotCore.Services.Entity
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewDTO> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Review>();

            var review = await repository
                .AsReadOnlyQueryable()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (review == null)
            {
                throw new KeyNotFoundException($"Review with id {id} not found");
            }

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllByMasterIdAsync(int masterId)
        {
            var repository = _unitOfWork.GetRepository<Review>();

            //Загружаем последние 15 отзывов
            var reviews = await repository
                .AsReadOnlyQueryable()
                .Where(b => b.MasterId == masterId)
                .Take(15)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> CreateAsync(ReviewDTO reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            review.CreatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<Review>().Create(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReviewDTO>(review);
        }
    }
}