using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Services;

namespace DealershipReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/reviews — public: anyone can read reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            return await _reviewService.GetAllAsync();
        }

        // GET: api/reviews/dealer/5 — public
        [HttpGet("dealer/{dealershipId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByDealer(int dealershipId)
        {
            return await _reviewService.GetByDealershipAsync(dealershipId);
        }

        // POST: api/reviews — requires a valid JWT
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto dto)
        {
            var created = await _reviewService.CreateAsync(dto);
            if (created == null)
                return BadRequest($"Dealership with id {dto.DealershipId} does not exist");

            return CreatedAtAction(nameof(GetReviews), new { id = created.Id }, created);
        }

        // DELETE: api/reviews/5 — requires a valid JWT
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var deleted = await _reviewService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
