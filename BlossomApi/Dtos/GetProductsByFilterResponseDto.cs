namespace BlossomApi.Dtos
{
    public class GetProductsByFilterResponseDto
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
        public double Rating { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfPurchases { get; set; }
        public int NumberOfViews { get; set; }
        public string Article { get; set; }
        public List<string> Options { get; set; }
        public List<string> Categories { get; set; }
        public List<int> DieNumbers { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public List<CharacteristicDto> Characteristics { get; set; }
        public string Description { get; set; }
        public bool InStock { get; set; }
    }

    public class ReviewDto
    {
        public string Name { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
        public string Date { get; set; }
    }

    public class CharacteristicDto
    {
        public string Title { get; set; }
        public string Desc { get; set; }
    }
}