using BlossomApi.Dtos.Product;

namespace BlossomApi.Dtos.Brends
{
    public class BrandWithProductsDto
    {
        public int BrandId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }
        public string ImageUrl { get; set; }
        public string LogoImageUrl { get; set; }
        public List<ProductCardDto> Products { get; set; } = new List<ProductCardDto>();
    }
}
