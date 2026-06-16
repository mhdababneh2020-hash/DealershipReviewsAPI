using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DealershipReviewsAPI.Data;
using DealershipReviewsAPI.Models;

namespace DealershipReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealershipsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DealershipsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/dealerships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dealership>>> GetDealerships()
        {
            return await _context.Dealerships.ToListAsync();
        }

        // GET: api/dealerships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dealership>> GetDealership(int id)
        {
            var dealership = await _context.Dealerships.FindAsync(id);
            if (dealership == null) return NotFound();
            return dealership;
        }

        // POST: api/dealerships
        [HttpPost]
        public async Task<ActionResult<Dealership>> CreateDealership(Dealership dealership)
        {
            _context.Dealerships.Add(dealership);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDealership), new { id = dealership.Id }, dealership);
        }

        // PUT: api/dealerships/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDealership(int id, Dealership dealership)
        {
            if (id != dealership.Id) return BadRequest();
            _context.Entry(dealership).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/dealerships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealership(int id)
        {
            var dealership = await _context.Dealerships.FindAsync(id);
            if (dealership == null) return NotFound();
            _context.Dealerships.Remove(dealership);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}