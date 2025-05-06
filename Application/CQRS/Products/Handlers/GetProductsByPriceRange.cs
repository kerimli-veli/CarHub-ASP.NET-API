using Application.CQRS.Products.ResponsesDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.Products.Handlers;

public class GetProductsByPriceRange
{
    
    public class ProductRangeCommand( decimal maxPrice) : IRequest<Result<List<GetByNameProductDto>>>
    {
        //public decimal MinPrice { get; set; } = minPrice;
        public decimal MaxPrice { get; set; } = maxPrice;
    }


    public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<ProductRangeCommand, Result<List<GetByNameProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<GetByNameProductDto>>> Handle(ProductRangeCommand request, CancellationToken cancellationToken)
        {

            //var products = await _unitOfWork.ProductRepository.GetProductsByPriceRange(request.MinPrice, request.MaxPrice);

            var products = await _unitOfWork.ProductRepository.GetProductsByPriceRange(0, request.MaxPrice);


            if (products == null || !products.Any())
            {

                return new Result<List<GetByNameProductDto>>
                {
                    Errors = new List<string> { "No products found in the given price range" },
                    IsSuccess = false
                };
            }

            var productDtos = _mapper.Map<List<GetByNameProductDto>>(products.ToList());


            return new Result<List<GetByNameProductDto>>
            {
                Data = productDtos,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}