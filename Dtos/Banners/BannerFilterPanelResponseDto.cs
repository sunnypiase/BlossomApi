using BlossomApi.Dtos.FilterPanel;

namespace BlossomApi.Dtos.Banners
{
    public class BannerFilterPanelResponseDto
    {
        public List<CategoryNode> Categories { get; set; } // List of root-level categories
        public List<FilterPanelCharacteristicDto> Characteristics { get; set; }
        public List<FilterPanelOptionDto> Brands { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
