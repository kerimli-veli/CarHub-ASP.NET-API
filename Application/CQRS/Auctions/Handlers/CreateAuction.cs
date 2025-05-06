using Application.CQRS.Auctions.ResponseDtos;
using Application.CQRS.Cars.ResponseDtos;
using AutoMapper;
using Azure;
using Azure.Core;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Auctions.Handler;

public class CreateAuction
{
    public class CreateAuctionCommand : IRequest<Result<AuctionResponseDto>>
    {
        public int CarId { get; set; }
        public int SellerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal StartingPrice { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateAuctionCommand, Result<AuctionResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<AuctionResponseDto>> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = new Domain.Entities.Auction
            {
                CarId = request.CarId,
                SellerId = request.SellerId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                StartingPrice = request.StartingPrice,
                IsActive = false
            };

            var car = await _unitOfWork.CarRepository.GetByIdAsync(request.CarId);

            var carDto = _mapper.Map<CarGetByIdDto>(car);

            await _unitOfWork.AuctionRepository.CreateAsync(auction);
            await _unitOfWork.SaveChangeAsync();

            var auctionResponseDto = _mapper.Map<AuctionResponseDto>(auction);

            auctionResponseDto.Car = carDto;

            return new Result<AuctionResponseDto>
            {
                Data = auctionResponseDto,
                Errors = [],
                IsSuccess = true
            };
        }
    }

}
 