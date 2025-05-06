namespace SignalR.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using MediatR;
    using Application.CQRS.SignalR.Handlers;

    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverUserId, string message)
        {
            var senderUserId = Context.UserIdentifier;

            if (string.IsNullOrEmpty(senderUserId))
            {
                await Clients.Caller.SendAsync("Error", "İstifadəçi id-si tapılmadı");
                return;
            }

            var sendMessageCommand = new SendMessage.SendMessageCommand
            {
                SenderId = int.Parse(senderUserId),
                ReceiverId = int.Parse(receiverUserId),
                Text = message
            };

            var result = await _mediator.Send(sendMessageCommand);

            if (result.IsSuccess)
            {
                await Clients.User(receiverUserId).SendAsync("ReceiveTyping", senderUserId);

                await Clients.Group(senderUserId).SendAsync("ReceiveMessage", senderUserId, receiverUserId, message);
                await Clients.Group(receiverUserId).SendAsync("ReceiveMessage", senderUserId, receiverUserId, message);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Mesaj göndərilərkən xəta baş verdi");
            }
        }

        public async Task SendTyping(string receiverUserId)
        {
            var senderUserId = Context.UserIdentifier;

            if (string.IsNullOrEmpty(senderUserId))
            {
                await Clients.Caller.SendAsync("Error", "İstifadəçi id-si tapılmadı");
                return;
            }

            await Clients.User(receiverUserId).SendAsync("ReceiveTyping", senderUserId);
        }
    }
}