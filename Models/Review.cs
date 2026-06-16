namespace DealershipReviewsAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int DealershipId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ReviewText { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Dealership? Dealership { get; set; }
    }
}