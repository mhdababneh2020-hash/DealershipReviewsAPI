using Microsoft.EntityFrameworkCore;
using DealershipReviewsAPI.Data;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Models;
using DealershipReviewsAPI.Services;

namespace DealershipReviewsAPI.Tests
{
    public class DealershipServiceTests
    {
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private static async Task SeedDealerships(AppDbContext context)
        {
            context.Dealerships.AddRange(
                new Dealership { Name = "Amman Motors", City = "Amman", State = "Jordan", Address = "Airport Road", ZipCode = "11118" },
                new Dealership { Name = "Irbid Cars", City = "Irbid", State = "Jordan", Address = "Main St", ZipCode = "21110" },
                new Dealership { Name = "Dubai Auto", City = "Dubai", State = "UAE", Address = "SZ Road", ZipCode = "00000" });
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task SearchAsync_ReturnsAll_WhenNoFilters()
        {
            using var context = CreateContext();
            await SeedDealerships(context);
            var service = new DealershipService(context);

            var result = await service.SearchAsync(null, null);

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task SearchAsync_FiltersByCity()
        {
            using var context = CreateContext();
            await SeedDealerships(context);
            var service = new DealershipService(context);

            var result = await service.SearchAsync("Amman", null);

            Assert.Single(result);
            Assert.Equal("Amman Motors", result[0].Name);
        }

        [Fact]
        public async Task SearchAsync_FiltersByState()
        {
            using var context = CreateContext();
            await SeedDealerships(context);
            var service = new DealershipService(context);

            var result = await service.SearchAsync(null, "Jordan");

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            using var context = CreateContext();
            var service = new DealershipService(context);

            var result = await service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ComputesAverageRating()
        {
            using var context = CreateContext();
            var dealership = new Dealership { Name = "Amman Motors", City = "Amman", State = "Jordan", Address = "Airport Road", ZipCode = "11118" };
            context.Dealerships.Add(dealership);
            context.Reviews.AddRange(
                new Review { Dealership = dealership, CustomerName = "A", ReviewText = "x", Rating = 5 },
                new Review { Dealership = dealership, CustomerName = "B", ReviewText = "y", Rating = 2 });
            await context.SaveChangesAsync();
            var service = new DealershipService(context);

            var result = await service.GetByIdAsync(dealership.Id);

            Assert.NotNull(result);
            Assert.Equal(3.5, result.AverageRating);
            Assert.Equal(2, result.ReviewCount);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            using var context = CreateContext();
            var service = new DealershipService(context);

            var result = await service.UpdateAsync(999, new UpdateDealershipDto
            {
                Name = "X", City = "Y", State = "Z", Address = "A", ZipCode = "1"
            });

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_ChangesFields_WhenFound()
        {
            using var context = CreateContext();
            var dealership = new Dealership { Name = "Old Name", City = "Amman", State = "Jordan", Address = "Old", ZipCode = "11118" };
            context.Dealerships.Add(dealership);
            await context.SaveChangesAsync();
            var service = new DealershipService(context);

            var result = await service.UpdateAsync(dealership.Id, new UpdateDealershipDto
            {
                Name = "New Name", City = "Amman", State = "Jordan", Address = "New", ZipCode = "11118"
            });

            Assert.True(result);
            var updated = await context.Dealerships.FindAsync(dealership.Id);
            Assert.Equal("New Name", updated!.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesDealership_WhenFound()
        {
            using var context = CreateContext();
            await SeedDealerships(context);
            var first = await context.Dealerships.FirstAsync();
            var service = new DealershipService(context);

            var result = await service.DeleteAsync(first.Id);

            Assert.True(result);
            Assert.Equal(2, await context.Dealerships.CountAsync());
        }
    }
}
