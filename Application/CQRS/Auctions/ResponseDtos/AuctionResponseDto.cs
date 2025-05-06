using Application.CQRS.Cars.ResponseDtos;
using Domain.Entities;

namespace Application.CQRS.Auctions.ResponseDtos;

public class AuctionResponseDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public CarGetByIdDto Car { get; set; }
    public int SellerId { get; set; }
    public decimal StartingPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsActive { get; set; }
}

