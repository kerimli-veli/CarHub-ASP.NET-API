using Domain.BaseEntities;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public string? CardNumber { get; set; }
    public string? ExpirationDate { get; set; }
    public string? CVV { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? Status { get; set; }
    public string? ShippingAddress { get; set; }

    public int UserId { get; set; }
    public decimal TotalPrice => OrderLines.Sum(ol => ol.TotalPrice);

    public DateTime OrderDate { get; set; }
    public User User { get; set; }
    public List<OrderLine> OrderLines { get; set; } = new();
}