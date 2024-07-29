namespace BlossomApi.Dtos
{
    public class GetProductsByFilterResponse
    {
        public List<ProductResponseDto> Products { get; set; }
        public int TotalCount { get; set; }
    }
}