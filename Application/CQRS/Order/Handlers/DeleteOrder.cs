using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Order.Handlers;

public class DeleteOrder
{
    public class OrderDeleteCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<OrderDeleteCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = await _unitOfWork.OrderRepository.DeleteOrderAsync(request.Id);
            if (!isDeleted)
            {
                return new Result<Unit>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Sifaris yoxdu" }
                };
            }

            await _unitOfWork.CompleteAsync(); // veya SaveChangeAsync

            return new Result<Unit>
            {
                IsSuccess = true,
                Data = Unit.Value
            };
        }
    }
}
