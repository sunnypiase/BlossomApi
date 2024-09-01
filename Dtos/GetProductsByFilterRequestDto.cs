using System.ComponentModel;

namespace BlossomApi.Dtos
{
    public class GetProductsByFilterRequestDto
    {
        public int? CategoryId { get; set; }
        public string? SortBy { get; set; }
        public bool? IsHit { get; set; }
        public bool? IsNew { get; set; }
        public bool? HasDiscount { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<int>? SelectedCharacteristics { get; set; }
        public int Start { get; set; }
        [DefaultValue(10)]
        public int Amount { get; set; }
    }
}