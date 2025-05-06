using Application.CQRS.Auctions.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Auctions.Handler;

public class GetBySellerIdAsync
{
    public class GetBySellerIdCommand : IRequest<Result<List<GetAllActiveAsyncDto>>> 
    {
        public int SellerId { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBySellerIdCommand, Result<List<GetAllActiveAsyncDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<GetAllActiveAsyncDto>>> Handle(GetBySellerIdCommand request, CancellationToken cancellationToken)
        {
            var auction = await _unitOfWork.AuctionRepository.GetBySellerIdAsync(request.SellerId);

            if (auction == null)
            {
                return new Result<List<GetAllActiveAsyncDto>>() { Errors = ["Auction not found"], IsSuccess = false };
            }

            var auctionResponseDto = _mapper.Map<List<GetAllActiveAsyncDto>>(auction);

            return new Result<List<GetAllActiveAsyncDto>>
            {
                Data = auctionResponseDto,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
