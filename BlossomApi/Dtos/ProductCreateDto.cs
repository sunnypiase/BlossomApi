namespace BlossomApi.Dtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string NameEng { get; set; }
        public List<string> Images { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsNew { get; set; }
        public double Rating { get; set; }
        public bool InStock { get; set; }
        public int AvailableAmount { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfPurchases { get; set; }
        public int NumberOfViews { get; set; }
        public string Article { get; set; }
        public List<string> Options { get; set; }
        public List<int> DieNumbers { get; set; }
        public string Description { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}