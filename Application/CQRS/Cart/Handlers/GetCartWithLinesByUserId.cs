using Application.CQRS.Cart.ResponseDtos;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using AutoMapper;

namespace Application.CQRS.Carts.Handlers;

public class GetCartWithLinesByUserId
{
    public class GetCartWithLinesByUserIdQuery : IRequest<Result<GetCartWithLinesByUserIdDto>>
    {
        public int UserId { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork, IMapper mapper): IRequestHandler<GetCartWithLinesByUserIdQuery, Result<GetCartWithLinesByUserIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<GetCartWithLinesByUserIdDto>> Handle(GetCartWithLinesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartWithLinesByUserId(request.UserId);

            if (cart == null)
            {
                return new Result<GetCartWithLinesByUserIdDto>
                {
                    Errors = { "User's cart not found" },
                    IsSuccess = false
                };
            }

            var response = _mapper.Map<GetCartWithLinesByUserIdDto>(cart);

            return new Result<GetCartWithLinesByUserIdDto>
            {
                Data = response,
                IsSuccess = true
            };
        }
    }
}