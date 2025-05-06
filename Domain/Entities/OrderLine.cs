using Domain.BaseEntities;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class OrderLine : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
    public Product Product { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }
}
