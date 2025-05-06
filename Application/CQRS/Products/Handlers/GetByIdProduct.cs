using Application.CQRS.Products.ResponsesDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using static Application.CQRS.Categories.Handlers.GetByIdCategory;

namespace Application.CQRS.Products.Handlers;

public class GetByIdProduct
{
    public class ProductGetByIdCommand : IRequest<Result<GetByIdProductDto>>
    {
        public int Id { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork) : IRequestHandler<ProductGetByIdCommand, Result<GetByIdProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<GetByIdProductDto>> Handle(ProductGetByIdCommand request, CancellationToken cancellationToken)
        {
            var currentproduct = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
            if (currentproduct == null)
            {
                return new Result<GetByIdProductDto>() { Errors = ["Product tapilmadi"], IsSuccess = true };
            }
            GetByIdProductDto response = new()
            {
                Id = currentproduct.Id,
                Name = currentproduct.Name,
                Description = currentproduct.Description,
                UnitPrice = currentproduct.UnitPrice,
                UnitsInStock = currentproduct.UnitsInStock,
                ImagePath = currentproduct.ImagePath,


            };

            return new Result<GetByIdProductDto> { Data = response, Errors = [], IsSuccess = true };
        }
    }
}

