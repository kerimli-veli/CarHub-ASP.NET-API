using Common.GlobalResponses.Generics;

using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers;

public sealed class Delete : IRequest<Result<bool>>
{
    public int Id { get; set; }

    public Delete(int id)
    {
        Id = id;
    }
}

public class DeleteProduct
{
    public class DeleteCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ProductRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangeAsync();
            return new Result<Unit> { Errors = [], IsSuccess = true };
        }
    }
}
