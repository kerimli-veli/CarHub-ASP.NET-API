using Application.CQRS.Order.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Order.Handlers;

public class GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<Result<List<OrderDto>>>
    {
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllOrdersQuery, Result<List<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllOrdersAsync();

            if (orders == null || !orders.Any())
            {
                return new Result<List<OrderDto>>
                {
                    Data = null,
                    Errors = new List<string> { "sifaris yoxdu" },
                    IsSuccess = false
                };
            }

            var dtos = _mapper.Map<List<OrderDto>>(orders);

            return new Result<List<OrderDto>>
            {
                Data = dtos,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}

