namespace BlossomApi.Dtos.Banners
{
    public class BannerUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AltText { get; set; }
        public List<int>? ProductIds { get; set; } = new();
        public IFormFile? DesktopImage { get; set; }
        public IFormFile? LaptopImage { get; set; }
        public IFormFile? TabletImage { get; set; }
        public IFormFile? PhoneImage { get; set; }
    }
}
