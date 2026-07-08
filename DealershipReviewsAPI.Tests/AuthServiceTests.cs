using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DealershipReviewsAPI.Data;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Services;

namespace DealershipReviewsAPI.Tests
{
    public class AuthServiceTests
    {
        // Each test gets its own isolated in-memory database
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        // Minimal JWT settings so the service can sign tokens in tests
        private static IConfiguration CreateConfig()
        {
            var settings = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "unit-test-signing-key-0123456789-0123456789-0123456789",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            };
            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }

        private static AuthService CreateService(AppDbContext context)
            => new(context, CreateConfig());

        private static RegisterDto SampleRegistration() => new()
        {
            Username = "ahmad",
            Password = "secret123",
            Email = "ahmad@example.com"
        };

        [Fact]
        public async Task RegisterAsync_StoresHashedPassword_NotPlaintext()
        {
            using var context = CreateContext();
            var service = CreateService(context);

            var error = await service.RegisterAsync(SampleRegistration());

            Assert.Null(error);
            var user = await context.Users.SingleAsync();
            Assert.NotEqual("secret123", user.PasswordHash);
            Assert.True(BCrypt.Net.BCrypt.Verify("secret123", user.PasswordHash));
        }

        [Fact]
        public async Task RegisterAsync_ReturnsError_WhenUsernameAlreadyExists()
        {
            using var context = CreateContext();
            var service = CreateService(context);
            await service.RegisterAsync(SampleRegistration());

            var error = await service.RegisterAsync(SampleRegistration());

            Assert.Equal("Username already exists", error);
            Assert.Equal(1, await context.Users.CountAsync());
        }

        [Fact]
        public async Task LoginAsync_ReturnsToken_WithCorrectCredentials()
        {
            using var context = CreateContext();
            var service = CreateService(context);
            await service.RegisterAsync(SampleRegistration());

            var result = await service.LoginAsync(new LoginDto
            {
                Username = "ahmad",
                Password = "secret123"
            });

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));

            // The token should be a valid JWT carrying the username claim
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.Token);
            Assert.Contains(jwt.Claims, c => c.Value == "ahmad");
            Assert.Equal("TestIssuer", jwt.Issuer);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WithWrongPassword()
        {
            using var context = CreateContext();
            var service = CreateService(context);
            await service.RegisterAsync(SampleRegistration());

            var result = await service.LoginAsync(new LoginDto
            {
                Username = "ahmad",
                Password = "wrong-password"
            });

            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            using var context = CreateContext();
            var service = CreateService(context);

            var result = await service.LoginAsync(new LoginDto
            {
                Username = "ghost",
                Password = "whatever"
            });

            Assert.Null(result);
        }
    }
}
