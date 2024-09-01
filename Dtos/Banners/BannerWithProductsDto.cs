using BlossomApi.Dtos.Product;

namespace BlossomApi.Dtos.Banners
{
    public class BannerWithProductsDto : BannerResponseDto
    {
        public List<ProductCardDto> Products { get; set; } = new();
    }
}
