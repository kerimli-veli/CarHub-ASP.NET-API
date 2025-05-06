using Application.CQRS.SignalR.ResponseDtos;
using AutoMapper;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.SignalR.Handlers;

public class GetMessages
{
    public class GetMessagesQuery : IRequest<List<ChatMessageDto>>
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public GetMessagesQuery(int senderId, int receiverId)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
        }
    }

    public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, List<ChatMessageDto>>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IMapper _mapper;

        public GetMessagesQueryHandler(IChatMessageRepository chatMessageRepository, IMapper mapper)
        {
            _chatMessageRepository = chatMessageRepository;
            _mapper = mapper;
        }

        public async Task<List<ChatMessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var chatMessages = await _chatMessageRepository.GetMessagesByUserIdsAsync(request.SenderId, request.ReceiverId);

            var chatMessageDtos = _mapper.Map<List<ChatMessageDto>>(chatMessages);

            return chatMessageDtos;
        }
    }

}
