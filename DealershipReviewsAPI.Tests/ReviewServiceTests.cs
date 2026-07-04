using Microsoft.EntityFrameworkCore;
using DealershipReviewsAPI.Data;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Models;
using DealershipReviewsAPI.Services;

namespace DealershipReviewsAPI.Tests
{
    public class ReviewServiceTests
    {
        // Each test gets its own isolated in-memory database
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private static async Task<Dealership> SeedDealership(AppDbContext context)
        {
            var dealership = new Dealership
            {
                Name = "Amman Motors",
                City = "Amman",
                State = "Jordan",
                Address = "Airport Road",
                ZipCode = "11118"
            };
            context.Dealerships.Add(dealership);
            await context.SaveChangesAsync();
            return dealership;
        }

        [Fact]
        public async Task CreateAsync_ReturnsNull_WhenDealershipDoesNotExist()
        {
            using var context = CreateContext();
            var service = new ReviewService(context);

            var result = await service.CreateAsync(new CreateReviewDto
            {
                DealershipId = 999,
                CustomerName = "Ahmad",
                ReviewText = "Great service",
                Rating = 5
            });

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_SavesReview_WhenDealershipExists()
        {
            using var context = CreateContext();
            var dealership = await SeedDealership(context);
            var service = new ReviewService(context);

            var result = await service.CreateAsync(new CreateReviewDto
            {
                DealershipId = dealership.Id,
                CustomerName = "Ahmad",
                ReviewText = "Great service",
                Rating = 5
            });

            Assert.NotNull(result);
            Assert.Equal("Amman Motors", result.DealershipName);
            Assert.Equal(5, result.Rating);
            Assert.Equal(1, await context.Reviews.CountAsync());
        }

        [Fact]
        public async Task GetByDealershipAsync_ReturnsOnlyMatchingReviews()
        {
            using var context = CreateContext();
            var dealership = await SeedDealership(context);
            var other = new Dealership { Name = "Irbid Cars", City = "Irbid", State = "Jordan", Address = "Main St", ZipCode = "21110" };
            context.Dealerships.Add(other);
            context.Reviews.AddRange(
                new Review { DealershipId = dealership.Id, CustomerName = "A", ReviewText = "x", Rating = 4 },
                new Review { DealershipId = dealership.Id, CustomerName = "B", ReviewText = "y", Rating = 2 },
                new Review { Dealership = other, CustomerName = "C", ReviewText = "z", Rating = 5 });
            await context.SaveChangesAsync();
            var service = new ReviewService(context);

            var result = await service.GetByDealershipAsync(dealership.Id);

            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(dealership.Id, r.DealershipId));
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenReviewNotFound()
        {
            using var context = CreateContext();
            var service = new ReviewService(context);

            var result = await service.DeleteAsync(999);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_RemovesReview_WhenItExists()
        {
            using var context = CreateContext();
            var dealership = await SeedDealership(context);
            var review = new Review { DealershipId = dealership.Id, CustomerName = "A", ReviewText = "x", Rating = 3 };
            context.Reviews.Add(review);
            await context.SaveChangesAsync();
            var service = new ReviewService(context);

            var result = await service.DeleteAsync(review.Id);

            Assert.True(result);
            Assert.Equal(0, await context.Reviews.CountAsync());
        }
    }
}
