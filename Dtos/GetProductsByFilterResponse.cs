using BlossomApi.Dtos.Product;

namespace BlossomApi.Dtos
{
    public class GetProductsByFilterResponse
    {
        public List<ProductCardDto> Products { get; set; }
        public int TotalCount { get; set; }
    }
}