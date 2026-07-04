using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Services;

namespace DealershipReviewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealershipsController : ControllerBase
    {
        private readonly IDealershipService _dealershipService;

        public DealershipsController(IDealershipService dealershipService)
        {
            _dealershipService = dealershipService;
        }

        // GET: api/dealerships?city=Amman&state=Jordan — public
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealershipDto>>> GetDealerships(
            [FromQuery] string? city,
            [FromQuery] string? state)
        {
            return await _dealershipService.SearchAsync(city, state);
        }

        // GET: api/dealerships/5 — public
        [HttpGet("{id}")]
        public async Task<ActionResult<DealershipDto>> GetDealership(int id)
        {
            var dealership = await _dealershipService.GetByIdAsync(id);
            if (dealership == null) return NotFound();
            return dealership;
        }

        // POST: api/dealerships — requires a valid JWT
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DealershipDto>> CreateDealership(CreateDealershipDto dto)
        {
            var created = await _dealershipService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetDealership), new { id = created.Id }, created);
        }

        // PUT: api/dealerships/5 — requires a valid JWT
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDealership(int id, UpdateDealershipDto dto)
        {
            var updated = await _dealershipService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE: api/dealerships/5 — requires a valid JWT
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealership(int id)
        {
            var deleted = await _dealershipService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
