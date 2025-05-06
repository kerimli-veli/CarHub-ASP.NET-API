using Application.CQRS.Notifications.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Notifications.Handlers;

public class GetAllNotifications
{
    public class GetAllNotificationsCommand : IRequest<Result<List<GetAllNotificationsDto>>>
    {
        public int UserId { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllNotificationsCommand, Result<List<GetAllNotificationsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<GetAllNotificationsDto>>> Handle(GetAllNotificationsCommand request, CancellationToken cancellationToken)
        {
            var notification = await _unitOfWork.NotificationRepository.GetAllNotification(request.UserId);

            if (notification == null)
            {
                return new Result<List<GetAllNotificationsDto>>() { Errors = ["Notification not found"], IsSuccess = false };
            }

            var response = _mapper.Map<List<GetAllNotificationsDto>>(notification);

            return new Result<List<GetAllNotificationsDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
