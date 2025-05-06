using Application.CQRS.Products.ResponsesDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Products.Handlers;

public class ProductResponse
{
    public class GetProductsByCategoryIdQuery : IRequest<Result<IEnumerable<ProductResponseDto>>>
    {
        public int CategoryId { get; set; }

        public GetProductsByCategoryIdQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }

    public class GetProductsByCategoryIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetProductsByCategoryIdQuery, Result<IEnumerable<ProductResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var products = _unitOfWork.ProductRepository.GetByCategoryId(request.CategoryId);

                if (products == null || !products.Any())
                {
                    return new Result<IEnumerable<ProductResponseDto>>
                    {
                        Data = null,
                        Errors = new List<string> { "No products found for the given category." },
                        IsSuccess = false
                    };
                }

                var productDtos = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

                return new Result<IEnumerable<ProductResponseDto>>
                {
                    Data = productDtos,
                    Errors = new List<string>(),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<ProductResponseDto>>
                {
                    Data = null,
                    Errors = new List<string> { ex.Message },
                    IsSuccess = false
                };
            }
        }
    }
}
