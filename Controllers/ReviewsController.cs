using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DealershipReviewsAPI.Data;
using DealershipReviewsAPI.Models;

namespace DealershipReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.Include(r => r.Dealership).ToListAsync();
        }

        // GET: api/reviews/dealer/5
        [HttpGet("dealer/{dealershipId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByDealer(int dealershipId)
        {
            return await _context.Reviews
                .Where(r => r.DealershipId == dealershipId)
                .Include(r => r.Dealership)
                .ToListAsync();
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReviews), new { id = review.Id }, review);
        }

        // DELETE: api/reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}