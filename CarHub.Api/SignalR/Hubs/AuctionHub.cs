using Microsoft.AspNetCore.SignalR;

namespace CarHub.Api.SignalR.Hubs;

public class AuctionHub : Hub
{
    public async Task SendAuctionNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveAuctionNotification", message);
    }
}

