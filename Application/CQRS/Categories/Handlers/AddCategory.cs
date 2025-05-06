using Application.CQRS.Categories.ResponseDtos;
using Application.Security;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers;

public class AddCategory
{
    public class AddCommand : IRequest<Result<CategoryAddDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddCommand, Result<CategoryAddDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CategoryAddDto>> Handle(AddCommand request, CancellationToken cancellationToken)
        {

            var newCategory = _mapper.Map<Domain.Entities.Category>(request);


            if (string.IsNullOrEmpty(newCategory.Name))
            {
                throw new Exception("Category name is required");
            }


            await _unitOfWork.CategoryRepository.AddAsync(newCategory);


            var response = _mapper.Map<CategoryAddDto>(newCategory);


            return new Result<CategoryAddDto>
            {
                Data = response,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}
