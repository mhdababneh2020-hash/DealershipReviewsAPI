using Microsoft.EntityFrameworkCore;
using DealershipReviewsAPI.Models;

namespace DealershipReviewsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dealership> Dealerships { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
    }
}