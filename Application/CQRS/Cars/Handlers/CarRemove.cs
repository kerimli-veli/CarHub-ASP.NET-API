using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Cars.Handlers;

public class CarRemove
{
    public class CarDeleteCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<CarDeleteCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(CarDeleteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.CarRepository.Remove(request.Id);
            await _unitOfWork.SaveChangeAsync();
            return new Result<Unit> { Errors = [], IsSuccess = true };
        }
    }
}
