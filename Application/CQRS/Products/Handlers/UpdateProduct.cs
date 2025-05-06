using Application.CQRS.Products.ResponsesDto;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers;

public class UpdateProduct
{
    public record struct UpdateProductCommand : IRequest<Result<UpdateProductDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string Description { get; set; }
        public List<string> ImagePath { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateProductCommand, Result<UpdateProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<UpdateProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentproduct = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
            if (currentproduct == null) throw new BadRequestException($"User does not exist with id {request.Id}");

            currentproduct.Name = request.Name;
            currentproduct.CategoryId = request.CategoryId;
            currentproduct.UnitPrice = request.UnitPrice;
            currentproduct.UnitsInStock = request.UnitsInStock;
            currentproduct.ImagePath = request.ImagePath;
            currentproduct.Description = request.Description;


            _unitOfWork.ProductRepository.Update(currentproduct);

            var response = _mapper.Map<UpdateProductDto>(currentproduct);

            return new Result<UpdateProductDto>()
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
