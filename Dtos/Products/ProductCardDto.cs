namespace BlossomApi.Dtos.Product
{
    public class ProductCardDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public decimal Price { get; set; }
        public bool IsHit { get; set; }
        public bool InStock { get; set; }
        public decimal Discount { get; set; }
    }

}
