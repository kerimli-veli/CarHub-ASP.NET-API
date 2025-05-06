using Application.CQRS.Order.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Order.Handlers;

public class GetOrderById
{
    public class GetOrderByIdQuery : IRequest<Result<OrderDto>>
    {
        public int OrderId { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(request.OrderId);

            if (order == null)
            {
                return new Result<OrderDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Sipariş tapilmadi" }
                };
            }

            var dto = _mapper.Map<OrderDto>(order);

            return new Result<OrderDto>
            {
                IsSuccess = true,
                Data = dto,
                Errors = new List<string>()
            };
        }
    }
}
