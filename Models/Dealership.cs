namespace DealershipReviewsAPI.Models
{
    public class Dealership
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public List<Review> Reviews { get; set; } = new();
        public double AverageRating => Reviews.Count > 0 
            ? Reviews.Average(r => r.Rating) 
            : 0;
    }
}