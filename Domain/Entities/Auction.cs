
using Domain.BaseEntities;

namespace Domain.Entities;

public class Auction : BaseEntity
{
    public int CarId { get; set; }
    public Car Car { get; set; }

    public int SellerId { get; set; }
    public User Seller { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public decimal StartingPrice { get; set; }
    public bool IsActive { get; set; } = true;
}
