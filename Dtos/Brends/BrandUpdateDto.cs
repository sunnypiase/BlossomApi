namespace BlossomApi.Dtos.Brends
{
    public class BrandUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? LogoImage { get; set; }
    }
}
