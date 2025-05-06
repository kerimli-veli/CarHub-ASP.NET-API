using Application.CQRS.Order.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Order.Handlers;

public class GetOrdersByUserId
{
    public class GetOrdersByUserIdQuery : IRequest<Result<List<OrderDto>>>
    {
        public int UserId { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetOrdersByUserIdQuery, Result<List<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(request.UserId);

            if (orders == null || !orders.Any())
            {
                return new Result<List<OrderDto>>
                {
                    Data = null,
                    Errors = new List<string> { "Userin sifarisi yoxdu" },
                    IsSuccess = false
                };
            }

            var dtoList = _mapper.Map<List<OrderDto>>(orders);

            return new Result<List<OrderDto>>
            {
                Data = dtoList,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}

