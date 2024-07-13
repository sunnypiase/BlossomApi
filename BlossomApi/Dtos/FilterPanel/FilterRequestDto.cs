namespace BlossomApi.Dtos.FilterPanel
{
    public class FilterRequestDto
    {
        public string CategoryName { get; set; }
        public List<int> SelectedCharacteristics { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}