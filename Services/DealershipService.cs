using Microsoft.EntityFrameworkCore;
using DealershipReviewsAPI.Data;
using DealershipReviewsAPI.Dtos;
using DealershipReviewsAPI.Models;

namespace DealershipReviewsAPI.Services
{
    public class DealershipService : IDealershipService
    {
        private readonly AppDbContext _context;

        public DealershipService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DealershipDto>> SearchAsync(string? city, string? state)
        {
            var query = _context.Dealerships.AsQueryable();

            if (!string.IsNullOrEmpty(city))
                query = query.Where(d => d.City.Contains(city));

            if (!string.IsNullOrEmpty(state))
                query = query.Where(d => d.State.Contains(state));

            // Projecting inside the query lets SQL Server compute the average
            // instead of loading every review into memory.
            return await query
                .Select(d => new DealershipDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    City = d.City,
                    State = d.State,
                    Address = d.Address,
                    ZipCode = d.ZipCode,
                    ReviewCount = d.Reviews.Count,
                    AverageRating = d.Reviews.Count > 0 ? d.Reviews.Average(r => r.Rating) : 0
                })
                .ToListAsync();
        }

        public async Task<DealershipDto?> GetByIdAsync(int id)
        {
            return await _context.Dealerships
                .Where(d => d.Id == id)
                .Select(d => new DealershipDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    City = d.City,
                    State = d.State,
                    Address = d.Address,
                    ZipCode = d.ZipCode,
                    ReviewCount = d.Reviews.Count,
                    AverageRating = d.Reviews.Count > 0 ? d.Reviews.Average(r => r.Rating) : 0
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DealershipDto> CreateAsync(CreateDealershipDto dto)
        {
            var dealership = new Dealership
            {
                Name = dto.Name,
                City = dto.City,
                State = dto.State,
                Address = dto.Address,
                ZipCode = dto.ZipCode
            };

            _context.Dealerships.Add(dealership);
            await _context.SaveChangesAsync();

            return new DealershipDto
            {
                Id = dealership.Id,
                Name = dealership.Name,
                City = dealership.City,
                State = dealership.State,
                Address = dealership.Address,
                ZipCode = dealership.ZipCode,
                ReviewCount = 0,
                AverageRating = 0
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateDealershipDto dto)
        {
            var dealership = await _context.Dealerships.FindAsync(id);
            if (dealership == null) return false;

            dealership.Name = dto.Name;
            dealership.City = dto.City;
            dealership.State = dto.State;
            dealership.Address = dto.Address;
            dealership.ZipCode = dto.ZipCode;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dealership = await _context.Dealerships.FindAsync(id);
            if (dealership == null) return false;

            _context.Dealerships.Remove(dealership);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
