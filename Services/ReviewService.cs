using Microsoft.EntityFrameworkCore;
using DealershipReviewsAPI.Data;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Models;

namespace DealershipReviewsAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewDto>> GetAllAsync()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Dealership)
                .ToListAsync();

            return reviews.Select(ToDto).ToList();
        }

        public async Task<List<ReviewDto>> GetByDealershipAsync(int dealershipId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.DealershipId == dealershipId)
                .Include(r => r.Dealership)
                .ToListAsync();

            return reviews.Select(ToDto).ToList();
        }

        public async Task<ReviewDto?> CreateAsync(CreateReviewDto dto)
        {
            var dealership = await _context.Dealerships.FindAsync(dto.DealershipId);
            if (dealership == null) return null;

            var review = new Review
            {
                DealershipId = dto.DealershipId,
                CustomerName = dto.CustomerName,
                ReviewText = dto.ReviewText,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            review.Dealership = dealership;
            return ToDto(review);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ReviewDto ToDto(Review r) => new()
        {
            Id = r.Id,
            DealershipId = r.DealershipId,
            DealershipName = r.Dealership != null ? r.Dealership.Name : string.Empty,
            CustomerName = r.CustomerName,
            ReviewText = r.ReviewText,
            Rating = r.Rating,
            CreatedAt = r.CreatedAt
        };
    }
}
