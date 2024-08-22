namespace BlossomApi.Dtos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public int Amount { get; set; }
        public List<string> Images { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsNew { get; set; }
        public bool IsHit { get; set; }
        public bool IsShown { get; set; }
        public double Rating { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfPurchases { get; set; }
        public int NumberOfViews { get; set; }
        public string Article { get; set; }
        public List<string> Options { get; set; }
        public List<CategoryResponseDto> Categories { get; set; }
        public List<int> DieNumbers { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public List<CharacteristicDto> Characteristics { get; set; }
        public string Description { get; set; }
        public bool InStock { get; set; }
    }
}