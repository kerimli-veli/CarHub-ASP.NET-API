using Application.CQRS.Cart.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Common;

namespace Application.CQRS.Cart.Handlers;

public class UpdateProductQuantityInCart
{
    public class UpdateProductQuantityInCartCommand : IRequest<Result<AddCartDto>>
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int QuantityChange { get; set; } 
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateProductQuantityInCartCommand, Result<AddCartDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<AddCartDto>> Handle(UpdateProductQuantityInCartCommand request, CancellationToken cancellationToken)
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
            var cartLine = cart.CartLines.FirstOrDefault(cl => cl.ProductId == request.ProductId && !cl.IsDeleted);

            if (cartLine != null)
            {
                cartLine.Quantity += request.QuantityChange;
                if (cartLine.Quantity <= 0)
                {
                    cartLine.IsDeleted = true;
                    cartLine.DeletedDate = DateTime.Now;
                    cartLine.DeletedBy = 0; 
                }
            }
            else if (request.QuantityChange > 0)
            {
                var newCartLine = new CartLine
                {
                    CartId = request.CartId,
                    ProductId = request.ProductId,
                    Quantity = request.QuantityChange,
                    UnitPrice = product.UnitPrice
                };

                cart.CartLines.Add(newCartLine);
            }
            else
            {
                return new Result<AddCartDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Product not found in cart and cannot decrease quantity" }
                };
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
