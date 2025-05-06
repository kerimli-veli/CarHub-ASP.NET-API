using Application.CQRS.Products.ResponsesDto;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Products.Handlers
{
    public class GetByNameProduct
    {
        public class ProductGetByNameQuery : IRequest<Result<List<GetByNameProductDto>>>
        {
            public string Name { get; set; }
        }

        public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<ProductGetByNameQuery, Result<List<GetByNameProductDto>>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<GetByNameProductDto>>> Handle(ProductGetByNameQuery request, CancellationToken cancellationToken)
            {
              
                var productsQuery = _unitOfWork.ProductRepository
                    .GetAll()
                    .Where(p => p.Name.ToLower().Contains(request.Name.ToLower()));

                var products = await productsQuery.ToListAsync();

                if (products == null || !products.Any())
                {
                    return new Result<List<GetByNameProductDto>>()
                    {
                        Errors = { "No products found matching the search criteria." },
                        IsSuccess = false
                    };
                }
                var response = _mapper.Map<List<GetByNameProductDto>>(products);
                return new Result<List<GetByNameProductDto>>
                {
                    Data = response,
                    Errors =  { },
                    IsSuccess = true
                };
            }
        }
    }
}
