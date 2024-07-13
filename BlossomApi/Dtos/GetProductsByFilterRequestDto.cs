namespace BlossomApi.Dtos
{
    public class GetProductsByFilterRequestDto
    {
        public string? CategoryName { get; set; }
        public string? SortBy { get; set; }
        public int Amount { get; set; }
        public int Start { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<int>? SelectedCharacteristics { get; set; }
    }
}