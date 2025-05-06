
using Application.CQRS.SignalR.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.SignalR.Handlers
{
    public class SendMessage
    {
        public class SendMessageCommand : IRequest<Result<ChatMessageDto>>
        {
            public int SenderId { get; set; }
            public int ReceiverId { get; set; }
            public string Text { get; set; }
        }

        public sealed class Handler : IRequestHandler<SendMessageCommand, Result<ChatMessageDto>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<ChatMessageDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
            {
                var message = new ChatMessage
                {
                    SenderId = request.SenderId,
                    ReceiverId = request.ReceiverId,
                    Text = request.Text,
                    SentAt = DateTime.UtcNow 
                };

                await _unitOfWork.ChatMessageRepository.AddAsync(message);

                var response = _mapper.Map<ChatMessageDto>(message);

                return new Result<ChatMessageDto>
                {
                    Data = response,
                    Errors = new List<string>(), 
                    IsSuccess = true
                };
            }
        }

    }
}

