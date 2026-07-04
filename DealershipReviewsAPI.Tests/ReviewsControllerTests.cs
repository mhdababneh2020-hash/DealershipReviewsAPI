using Microsoft.AspNetCore.Mvc;
using Moq;
using DealershipReviewsAPI.Controllers;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Services;

namespace DealershipReviewsAPI.Tests
{
    // Controller tests use a mocked service — no database involved
    public class ReviewsControllerTests
    {
        private readonly Mock<IReviewService> _serviceMock;
        private readonly ReviewsController _controller;

        public ReviewsControllerTests()
        {
            _serviceMock = new Mock<IReviewService>();
            _controller = new ReviewsController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetReviews_ReturnsListFromService()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ReviewDto>
            {
                new() { Id = 1, CustomerName = "Ahmad", Rating = 5 },
                new() { Id = 2, CustomerName = "Sara", Rating = 3 }
            });

            var result = await _controller.GetReviews();

            var list = Assert.IsAssignableFrom<IEnumerable<ReviewDto>>(result.Value);
            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task CreateReview_ReturnsBadRequest_WhenDealershipMissing()
        {
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<CreateReviewDto>()))
                        .ReturnsAsync((ReviewDto?)null);

            var result = await _controller.CreateReview(new CreateReviewDto { DealershipId = 999 });

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateReview_ReturnsCreated_WhenSuccessful()
        {
            var created = new ReviewDto { Id = 7, DealershipId = 1, CustomerName = "Ahmad", Rating = 5 };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<CreateReviewDto>()))
                        .ReturnsAsync(created);

            var result = await _controller.CreateReview(new CreateReviewDto { DealershipId = 1 });

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var dto = Assert.IsType<ReviewDto>(createdResult.Value);
            Assert.Equal(7, dto.Id);
        }

        [Fact]
        public async Task DeleteReview_ReturnsNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

            var result = await _controller.DeleteReview(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteReview_ReturnsNoContent_WhenDeleted()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteReview(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
