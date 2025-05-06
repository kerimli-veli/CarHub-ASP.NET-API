using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using Application.Services;
using Application.CQRS.Auctions.ResponseDtos;
using AutoMapper;

namespace Application.CQRS.Auctions.Handlers;

public class SetIsActiveAsync
{
    public class SetIsActiveCommand : IRequest<Result<AuctionActivatedNotificationDto>>
    {
        public int AuctionId { get; set; }
    }

    public sealed class Handler(
        IUnitOfWork unitOfWork,
        INotificationService notificationService,
        IMapper mapper)
        : IRequestHandler<SetIsActiveCommand, Result<AuctionActivatedNotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<AuctionActivatedNotificationDto>> Handle(SetIsActiveCommand request, CancellationToken cancellationToken)
        {
            var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(request.AuctionId);

            var success = await _unitOfWork.AuctionRepository.SetIsActiveAsync(request.AuctionId);

            if (success == null)
            {
                return new Result<AuctionActivatedNotificationDto>
                {
                    IsSuccess = false,
                    Errors = ["Auction not found or failed to activate."]
                };
            }

            var auctionNotificationDto = _mapper.Map<AuctionActivatedNotificationDto>(auction);

            await _notificationService.SendAuctionActivatedNotificationAsync(auctionNotificationDto);

            return new Result<AuctionActivatedNotificationDto>
            {
                Data = auctionNotificationDto,
                IsSuccess = true
            };
        }
    }
}
