using Application.CQRS.Auctions.ResponseDtos;
using Application.CQRS.Cars.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Auctions.Handler;

public class AuctionDelete
{
    public class DeleteAuctionCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteAuctionCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _unitOfWork.AuctionRepository.DeleteAsync(request.Id);

            if (auction == null)
            {
                return new Result<Unit>() { Errors = ["Auction not found"], IsSuccess = false };
            }

            await _unitOfWork.SaveChangeAsync(); 
            return new Result<Unit>
            {
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
