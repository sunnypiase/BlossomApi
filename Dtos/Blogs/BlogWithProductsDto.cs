using BlossomApi.Dtos.Product;

namespace BlossomApi.Dtos.Blogs
{
    public class BlogWithProductsDto
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }
        public string DesktopImageUrl { get; set; }
        public string LaptopImageUrl { get; set; }
        public string TabletImageUrl { get; set; }
        public string PhoneImageUrl { get; set; }
        public string AltText { get; set; }
        public List<ProductCardDto> Products { get; set; } = new List<ProductCardDto>();
    }
}
