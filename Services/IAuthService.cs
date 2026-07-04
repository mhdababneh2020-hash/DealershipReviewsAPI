using DealershipReviewsAPI.Dtos;

namespace DealershipReviewsAPI.Services
{
    public interface IAuthService
    {
        // Returns an error message on failure, null on success.
        Task<string?> RegisterAsync(RegisterDto dto);
        // Returns a token on success, null on invalid credentials.
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
