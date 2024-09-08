using System.ComponentModel.DataAnnotations;

namespace BlossomApi.Dtos.Banners
{
    public class BannerCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile DesktopImage { get; set; }

        [Required]
        public IFormFile LaptopImage { get; set; }

        [Required]
        public IFormFile TabletImage { get; set; }

        [Required]
        public IFormFile PhoneImage { get; set; }

        [Required]
        public string AltText { get; set; }

        public List<int> ProductIds { get; set; } = new();
    }

}
