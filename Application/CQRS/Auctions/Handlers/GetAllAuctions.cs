using Application.CQRS.Auctions.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Auctions.Handlers;

public class GetAllAuctions
{
    public class GetAllAuctionsCommand : IRequest<Result<List<GetAllActiveAsyncDto>>> { }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllAuctionsCommand, Result<List<GetAllActiveAsyncDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<GetAllActiveAsyncDto>>> Handle(GetAllAuctionsCommand request, CancellationToken cancellationToken)
        {
            var auction = await _unitOfWork.AuctionRepository.GetAllAsync();

            if (auction == null)
            {
                return new Result<List<GetAllActiveAsyncDto>>() { Errors = ["Auctions not found"], IsSuccess = false };
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
