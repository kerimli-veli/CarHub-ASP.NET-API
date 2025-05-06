
using Application.CQRS.Auctions.ResponseDtos;
using Application.CQRS.Cars.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Auctions.Handler;

public class GetAllActiveAsync
{
    public class GetAllActiveCommand : IRequest<Result<List<GetAllActiveAsyncDto>>> {}

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllActiveCommand, Result<List<GetAllActiveAsyncDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<GetAllActiveAsyncDto>>> Handle(GetAllActiveCommand request, CancellationToken cancellationToken)
        {
            var auction = await _unitOfWork.AuctionRepository.GetAllActiveAsync();

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
