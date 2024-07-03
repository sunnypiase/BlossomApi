namespace BlossomApi.Dtos
{
    public class GetProductsByFilterRequestDto
    {
        public List<string> Categories { get; set; }
        public string? SortBy { get; set; }
        public int Amount { get; set; }
        public int Start { get; set; }
        public decimal? MinPrice { get; set; } // Add MinPrice
        public decimal? MaxPrice { get; set; } // Add MaxPrice
        public string? Search { get; set; }
    }
}