using Application.CQRS.Products.ResponsesDto;
using AutoMapper;
using Common.GlobalResponses;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers;

public class AddProduct
{
    public class AddProductCommand : IRequest<Result<AddProductDto>>
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string Description { get; set; }
        public List<string> ImagePath { get; set; }
        public int CategoryId { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddProductCommand, Result<AddProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<AddProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.CategoryId);
            //if (product == null)
            //{
            //    return new Result<AddProductDto>
            //    {
            //        Data = null,
            //        Errors = new List<string> { "Kategori Null." },
            //        IsSuccess = false
            //    };
            //}

            var newProduct = _mapper.Map<Product>(request);

            newProduct.CategoryId = request.CategoryId;


            await _unitOfWork.ProductRepository.AddAsync(newProduct);


            await _unitOfWork.CompleteAsync();


            var response = _mapper.Map<AddProductDto>(newProduct);

            return new Result<AddProductDto>
            {
                Data = response,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}



