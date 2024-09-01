using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlossomApi.Dtos.Banners
{
    public class GetProductsByBannerFilterRequestDto
    {
        [Required]
        public int BannerId { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<int>? SelectedCharacteristics { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public int Start { get; set; }
        [DefaultValue(10)]
        public int Amount { get; set; }
    }

}
