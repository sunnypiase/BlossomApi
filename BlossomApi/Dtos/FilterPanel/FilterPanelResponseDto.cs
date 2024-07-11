namespace BlossomApi.Dtos.FilterPanel
{
    public class FilterPanelResponseDto
    {
        public List<string> Categories { get; set; }
        public List<FilterPanelCharacteristicDto> Characteristics { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}