using BlossomApi.Models;

namespace BlossomApi.Seeders
{
    public static class DatabaseReviewSeeder
    {
        public static List<Review> GetReviews()
        {
            var reviews = new List<Review>();
            var random = new Random();
            var reviewId = 1;
            // Generate reviews for each product
            for (int productId = 1; productId < 100; productId++)
            {
                int numberOfReviews = random.Next(1, 6); // Generate between 1 and 5 reviews per product
                for (int i = 0; i < numberOfReviews; i++)
                {
                    reviews.Add(new Review
                    {
                        ReviewId = reviewId++,
                        Name = $"Reviewer {random.Next(1, 100)}",
                        ReviewText = "This is a sample review.",
                        Rating = random.Next(1, 6), // Random rating between 1 and 5
                        Date = DateTime.Now.AddDays(-random.Next(0, 365)), // Random date within the past year
                        ProductId = productId
                    });
                }
            }

            return reviews;
        }
    }
}