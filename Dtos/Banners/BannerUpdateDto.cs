namespace BlossomApi.Dtos.Banners
{
    public class BannerUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? DesktopAltText { get; set; }
        public string? LaptopAltText { get; set; }
        public string? TabletAltText { get; set; }
        public string? PhoneAltText { get; set; }
        public List<int>? ProductIds { get; set; } = new();
        public IFormFile? DesktopImage { get; set; }
        public IFormFile? LaptopImage { get; set; }
        public IFormFile? TabletImage { get; set; }
        public IFormFile? PhoneImage { get; set; }
    }
}
