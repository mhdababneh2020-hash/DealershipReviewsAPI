using System.ComponentModel.DataAnnotations;

namespace DealershipReviewsAPI.Dtos
{
    public class DealershipDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }

    public class CreateDealershipDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; } = string.Empty;
    }

    // Kept separate from CreateDealershipDto so update rules can evolve independently
    public class UpdateDealershipDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; } = string.Empty;
    }
}
