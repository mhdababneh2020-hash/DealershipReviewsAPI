using DealershipReviewsAPI.Dtos;

namespace DealershipReviewsAPI.Services
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetAllAsync();
        Task<List<ReviewDto>> GetByDealershipAsync(int dealershipId);
        // Returns null when the target dealership does not exist.
        Task<ReviewDto?> CreateAsync(CreateReviewDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
