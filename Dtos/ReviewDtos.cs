using System.ComponentModel.DataAnnotations;

namespace DealershipReviewsAPI.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int DealershipId { get; set; }
        public string DealershipName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ReviewText { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Id and CreatedAt are set by the server, not the client
    public class CreateReviewDto
    {
        [Required]
        public int DealershipId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string ReviewText { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
    }
}
