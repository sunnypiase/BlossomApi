using System.ComponentModel.DataAnnotations;

namespace BlossomApi.Dtos.Brends
{
    public class BrandCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public IFormFile LogoImage { get; set; }
    }
}
