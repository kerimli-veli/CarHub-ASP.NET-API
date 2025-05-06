using Application.CQRS.SignalR.Handlers;
using Application.CQRS.SignalR.ResponseDtos;
using Common.GlobalResponses.Generics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.SignalR.Handlers.GetMessages;

namespace CarHub.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class ChatController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("send")]
        public async Task<ActionResult<Result<ChatMessageDto>>> SendMessage([FromBody] SendMessage.SendMessageCommand command)
        {
            var result = await _sender.Send(command);
            return Ok(result);
        }

    [HttpGet("getMessages")]
    public async Task<IActionResult> GetMessages([FromQuery] int senderId, [FromQuery] int receiverId)
    {
        var query = new GetMessagesQuery(senderId, receiverId);
        Console.WriteLine($"SenderId: {senderId}, ReceiverId: {receiverId}"); 
        var messages = await _sender.Send(query);
        Console.WriteLine($"Mesajlar: {messages.Count()}"); 
        return Ok(messages);
    }

}
