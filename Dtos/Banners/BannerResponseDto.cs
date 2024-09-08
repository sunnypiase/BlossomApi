namespace BlossomApi.Dtos.Banners
{
    public class BannerResponseDto
    {
        public int BannerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DesktopImageUrl { get; set; }
        public string LaptopImageUrl { get; set; }
        public string TabletImageUrl { get; set; }
        public string PhoneImageUrl { get; set; }
        public string AltText { get; set; }
    }
}
