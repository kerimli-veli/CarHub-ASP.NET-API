using Domain.Entities;

namespace Application.CQRS.Cart.ResponseDtos;

public  class AddProductToCartDto
{
    public int UserId { get; set; }
    public decimal TotalPrice => CartLines.Sum(cl => cl.TotalPrice);
    public List<CartLine> CartLines { get; set; } = new List<CartLine>();
}
