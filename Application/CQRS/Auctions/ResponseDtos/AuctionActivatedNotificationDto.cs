using Application.CQRS.Cars.ResponseDtos;
using Application.CQRS.Users.ResponseDtos;
using Domain.Entities;

namespace Application.CQRS.Auctions.ResponseDtos;

public class AuctionActivatedNotificationDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public CarGetByIdDto Car { get; set; }
    public GetByIdDto User { get; set; }
    public int SellerId { get; set; }
    public decimal StartingPrice { get; set; }
    public bool IsActive { get; set; }
}
