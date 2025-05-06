using Domain.Entities;

namespace Application.CQRS.Cart.ResponseDtos;

public class GetCartWithLinesByUserIdDto
{
    public int UserId { get; set; }
    public int CartId { get; set; }
    public decimal TotalPrice => CartLines.Sum(cl => cl.TotalPrice);
    public List<CartLine> CartLines { get; set; } = new List<CartLine>();
}
