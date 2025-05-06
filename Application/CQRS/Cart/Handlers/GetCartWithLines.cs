using Application.CQRS.Cart.ResponseDtos;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Cart.Handlers;

public class GetCartWithLines
{

    public class GetCartWithLinesQuery : IRequest<Result<GetCartWithLinesDto>>
    {
        public int CartId { get; set; }
    }
    
    public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<GetCartWithLinesQuery, Result<GetCartWithLinesDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<GetCartWithLinesDto>> Handle(GetCartWithLinesQuery request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartWithLinesAsync(request.CartId);
            if (cart == null)
            {
                return new Result<GetCartWithLinesDto>()
                {
                    Errors = { "Cartline's not found" },
                    IsSuccess = false
                };
            }
            var response = new GetCartWithLinesDto
            {
                UserId = cart.UserId,
                CartLines = cart.CartLines.Select(cl => new Domain.Entities.CartLine
                {
                    Id = cl.Id,
                    ProductId = cl.ProductId,
                    Quantity = cl.Quantity,
                    UnitPrice = cl.UnitPrice
                }).ToList()
            };
            return new Result<GetCartWithLinesDto>
            {
                Data = response,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}
