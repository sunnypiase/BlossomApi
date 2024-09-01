namespace BlossomApi.Dtos.Blogs
{
    public class BlogUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }
        public IFormFile? DesktopImage { get; set; }
        public IFormFile? LaptopImage { get; set; }
        public IFormFile? TabletImage { get; set; }
        public IFormFile? PhoneImage { get; set; }
        public string? DesktopAltText { get; set; }
        public string? LaptopAltText { get; set; }
        public string? TabletAltText { get; set; }
        public string? PhoneAltText { get; set; }
        public List<int>? ProductIds { get; set; }
    }
}
