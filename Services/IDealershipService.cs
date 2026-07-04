using DealershipReviewsAPI.Dtos;

namespace DealershipReviewsAPI.Services
{
    public interface IDealershipService
    {
        Task<List<DealershipDto>> SearchAsync(string? city, string? state);
        Task<DealershipDto?> GetByIdAsync(int id);
        Task<DealershipDto> CreateAsync(CreateDealershipDto dto);
        Task<bool> UpdateAsync(int id, UpdateDealershipDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
