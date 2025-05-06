using Application.CQRS.Order.ResponseDtos;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using AutoMapper;

namespace Application.CQRS.Order.Commands;

public class UpdateOrderStatusCommand : IRequest<Result<OrderDto>>
{
    public int? OrderId { get; set; }
    public int UserId { get; set; }
    public string NewStatus { get; set; }

    public class Handler : IRequestHandler<UpdateOrderStatusCommand, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = request.OrderId.HasValue
    ? await _unitOfWork.OrderRepository.GetOrderByIdAsync(request.OrderId.Value)
    : (await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(request.UserId)).FirstOrDefault(); // varsayalım son siparişi çekiyor

            if (order == null || order.IsDeleted)
            {
                return new Result<OrderDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Sifariş tapılmadı və ya silinmişdir." }
                };
            }

            order.Status = request.NewStatus;
            order.UpdatedDate = DateTime.Now;

            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<OrderDto>(order);
            return new Result<OrderDto> { IsSuccess = true, Data = dto };
        }
    }
}
