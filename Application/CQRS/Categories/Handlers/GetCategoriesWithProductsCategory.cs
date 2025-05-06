using Application.CQRS.Categories.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Categories.Handlers
{

    public record GetCategoriesWithProductsQuery : IRequest<Result<List<GetCategoriesWithProductsDto>>> { }
    public sealed class GetCategoriesWithProductsHandler : IRequestHandler<GetCategoriesWithProductsQuery, Result<List<GetCategoriesWithProductsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCategoriesWithProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<GetCategoriesWithProductsDto>>> Handle(GetCategoriesWithProductsQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesWithProducts();
            if (categories == null || !categories.Any())
            {
                return new Result<List<GetCategoriesWithProductsDto>>
                {
                    Data = new List<GetCategoriesWithProductsDto>(),
                    Errors = new List<string> { "No categories found" },
                    IsSuccess = false
                };
            }
            var response = _mapper.Map<List<GetCategoriesWithProductsDto>>(categories);
            return new Result<List<GetCategoriesWithProductsDto>>
            {
                Data = response,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}
