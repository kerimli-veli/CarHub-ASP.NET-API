using Application.CQRS.Cart.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Cart.Queries;

public class GetCartTotalPrice
{
    public class GetCartTotalPriceQuery(int cartId) : IRequest<Result<GetTotalPriceDto>>
    {
        public int CartId { get; set; } = cartId;
    }

    public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCartTotalPriceQuery, Result<GetTotalPriceDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<GetTotalPriceDto>> Handle(GetCartTotalPriceQuery request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartWithLinesAsync(request.CartId);

            if (cart == null)
            {
                return new Result<GetTotalPriceDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Cart not found" }
                };
            }

            var dto = _mapper.Map<GetTotalPriceDto>(cart);

            return new Result<GetTotalPriceDto>
            {
                Data = dto,
                IsSuccess = true
            };
        }
    }
}
