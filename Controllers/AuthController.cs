using Microsoft.AspNetCore.Mvc;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Services;

namespace DealershipReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register — public by definition
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var error = await _authService.RegisterAsync(dto);
            if (error != null) return BadRequest(error);
            return Ok("User registered successfully");
        }

        // POST: api/auth/login — public by definition
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null) return Unauthorized("Invalid credentials");
            return result;
        }
    }
}
