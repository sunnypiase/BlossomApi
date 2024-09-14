namespace BlossomApi.Dtos.Brends
{
    public class BrandResponseDto
    {
        public int BrandId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }
        public string ImageUrl { get; set; }
        public string LogoImageUrl { get; set; }
    }
}
