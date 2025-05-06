using Application.CQRS.Cart.ResponseDtos;
using AutoMapper;
using Azure;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Common;

namespace Application.CQRS.Cart.Handlers;

public class AddProductToCart
{
    public class AddProductToCartCommand : IRequest<Result<AddCartDto>>
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddProductToCartCommand, Result<AddCartDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<AddCartDto>> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartWithLinesAsync(request.CartId);

            if (cart == null)
            {
                return new Result<AddCartDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Cart not found" }
                };
            }

            var product = await _unitOfWork.ProductRepository.GetAll().FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
            {
                return new Result<AddCartDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Product not found" }
                };
            }
            var existingCartLine = cart.CartLines.FirstOrDefault(cl => cl.ProductId == request.ProductId);

            if (existingCartLine != null)
            {
               
                existingCartLine.Quantity += request.Quantity;
            }
            else
            {
               
                var newCartLine = new CartLine
                {
                    CartId = request.CartId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = product.UnitPrice 
                };

                cart.CartLines.Add(newCartLine);
            }

            
            
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<AddCartDto>(cart);

            return new Result<AddCartDto>
            {
                Data = dto,
                IsSuccess = true
            };
        }
    }
}

