namespace Application.CQRS.Categories.ResponseDtos;

public class GetCategoriesWithProductsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ProductDto> Products { get; set; }
}
