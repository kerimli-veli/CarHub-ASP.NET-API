using Domain.BaseEntities;


namespace Domain.Entities;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }

    public List<Product> Products { get; set; } = new List<Product>();



}
