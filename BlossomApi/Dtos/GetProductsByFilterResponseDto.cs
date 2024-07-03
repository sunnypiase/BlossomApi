namespace BlossomApi.Dtos
{
    public class GetProductsByFilterResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public string Image { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsNew { get; set; }
        public double Rating { get; set; }
        public bool InStock { get; set; }
        public List<string> Categories { get; set; }
    }
}