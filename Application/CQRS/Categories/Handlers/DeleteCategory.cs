using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers;

public sealed class DeleteCategory : IRequest<Result<bool>>
{
    public int Id { get; set; }

    public DeleteCategory(int id)
    {
        Id = id;
    }
}

public class Remove
{
    public class CategoryDeleteCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork) : IRequestHandler<CategoryDeleteCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.CategoryRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangeAsync();
            return new Result<Unit> { Errors = [], IsSuccess = true };
        }
    }
}
