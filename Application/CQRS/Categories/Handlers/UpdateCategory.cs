using Application.CQRS.Categories.ResponseDtos;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers;

public class UpdateCategory
{
    public record struct CategoryCommand : IRequest<Result<CategoryUpdateDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CategoryCommand, Result<CategoryUpdateDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CategoryUpdateDto>> Handle(CategoryCommand request, CancellationToken cancellationToken)
        {
            var currentCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
            if (currentCategory == null) throw new BadRequestException($"User does not exist with id {request.Id}");

            currentCategory.Name = request.Name;
            currentCategory.Description = request.Description;
            

            _unitOfWork.CategoryRepository.Update(currentCategory);

            var response = _mapper.Map<CategoryUpdateDto>(currentCategory);

            return new Result<CategoryUpdateDto>()
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
