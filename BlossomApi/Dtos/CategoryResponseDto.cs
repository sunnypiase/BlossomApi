namespace BlossomApi.Dtos
{
    public class CategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
    }
}