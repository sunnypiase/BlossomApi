namespace BlossomApi.Dtos.Reviews
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int SiteUserId { get; set; }
        public string Username { get; set; }
    }
}
