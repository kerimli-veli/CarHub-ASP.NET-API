using Application.CQRS.Categories.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers;



public class GetAllCategory
{
    public record struct GetAllCategoryQuery : IRequest<Result<List<CategoryGetAllDto>>> { }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllCategoryQuery, Result<List<CategoryGetAllDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<CategoryGetAllDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();
            if (categories == null || !categories.Any())
                return new Result<List<CategoryGetAllDto>>
                {
                    Data = [],
                    Errors = ["No cars found"],
                    IsSuccess = false
                };

            var response = _mapper.Map<List<CategoryGetAllDto>>(categories);

            return new Result<List<CategoryGetAllDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}

