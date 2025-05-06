using Application.CQRS.Auctions.ResponseDtos;
using Application.Services;
using CarHub.Api.SignalR.Hubs;
using DAL.SqlServer.Context;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace CarHub.Api.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly AppDbContext _context;

    public NotificationService(IHubContext<NotificationHub> hubContext, AppDbContext context)
    {
        _hubContext = hubContext;
        _context = context;
    }

    public async Task SendAuctionActivatedNotificationAsync(AuctionActivatedNotificationDto data)
    {
        var users = _context.Users.ToList();

        foreach (var user in users)
        {
            var notification = new Notification
            {
                UserId = user.Id,
                Title = "Auction Notification",
                Message = $"{data.Car.Brand} {data.Car.Model} have been auction",
            };

            _context.Notifications.Add(notification);
        }

        await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
        {
            id = Guid.NewGuid().ToString(),
            message = $"{data.Car.Brand} {data.Car.Model} have been auction"
        });

        await _context.SaveChangesAsync();
    }


}

