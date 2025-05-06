using Domain.BaseEntities;

namespace Domain.Entities;

public class Cart : BaseEntity
{
    public int UserId { get; set; }
    public decimal TotalPrice => CartLines.Sum(cl => cl.TotalPrice);
    public User User { get; set; }
    public List<CartLine> CartLines { get; set; } = new();


}
