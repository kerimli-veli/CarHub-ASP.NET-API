using Application.CQRS.Categories.ResponseDtos;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers;

public class GetByIdCategory
{
    public class CategoryGetByIdCommand : IRequest<Result<CategoryGetByIdDto>>
    {
        public int Id { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork) : IRequestHandler<CategoryGetByIdCommand, Result<CategoryGetByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<CategoryGetByIdDto>> Handle(CategoryGetByIdCommand request, CancellationToken cancellationToken)
        {
            var currentCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
            if (currentCategory == null)
            {
                return new Result<CategoryGetByIdDto>() { Errors = ["User tapilmadi"], IsSuccess = true };
            }
            CategoryGetByIdDto response = new()
            {
                Id = currentCategory.Id,
                Name = currentCategory.Name,
                Description = currentCategory.Description

            };

            return new Result<CategoryGetByIdDto> { Data = response, Errors = [], IsSuccess = true };
        }
    }
}
