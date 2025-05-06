using Application.CQRS.Auctions.ResponseDtos;

namespace Application.Services;

public interface INotificationService
{
    Task SendAuctionActivatedNotificationAsync(AuctionActivatedNotificationDto data);
}
