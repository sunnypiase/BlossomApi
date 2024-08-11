namespace BlossomApi.Dtos
{
    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public string? NameEng { get; set; }
        public string? Brand { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public bool? IsNew { get; set; }
        public int? AvailableAmount { get; set; }
        public string? Article { get; set; }
        public List<int>? DieNumbers { get; set; }
        public string? Description { get; set; }
        public List<int>? CategoryIds { get; set; }
    }
}