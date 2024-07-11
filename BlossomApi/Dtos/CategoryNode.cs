namespace BlossomApi.Dtos;

public class CategoryNode
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public List<CategoryNode> Children { get; set; }
}