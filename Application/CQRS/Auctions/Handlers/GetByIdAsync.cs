using Application.CQRS.Auctions.ResponseDtos;
using Application.CQRS.Cars.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Auctions.Handler;

public class GetByIdAsync
{
    public class GetByIdAuctionCommand : IRequest<Result<AuctionResponseDto>>
    {
        public int AuctionId { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetByIdAuctionCommand, Result<AuctionResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<AuctionResponseDto>> Handle(GetByIdAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _unitOfWork.AuctionRepository.GetByIdAsync(request.AuctionId);

            if (auction == null)
            {
                return new Result<AuctionResponseDto>() { Errors = ["Auction not found"], IsSuccess = false };
            }

            var auctionResponseDto = _mapper.Map<AuctionResponseDto>(auction);
            auctionResponseDto.Car = _mapper.Map<CarGetByIdDto>(auction.Car);

            return new Result<AuctionResponseDto>
            {
                Data = auctionResponseDto,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
