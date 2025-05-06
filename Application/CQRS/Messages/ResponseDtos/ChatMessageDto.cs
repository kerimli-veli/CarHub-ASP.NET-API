namespace Application.CQRS.SignalR.ResponseDtos;

public class ChatMessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Text { get; set; }
    public DateTime SentAt { get; set; }
}

