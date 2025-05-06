using Application.CQRS.Users.ResponseDtos;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using static Application.CQRS.Users.Handlers.GetById;

namespace Application.CQRS.Users.Handlers;

public class GetUserByEmail
{
    public class GetUserByEmailCommand : IRequest<Result<GetUserByEmailDto>>
    {
        public string Email { get; set; }
    }
    public sealed class Handler : IRequestHandler<GetUserByEmailCommand, Result<GetUserByEmailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetUserByEmailDto>> Handle(GetUserByEmailCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);

            if (currentUser == null)
            {
                return new Result<GetUserByEmailDto>() { Errors = ["User not found"], IsSuccess = false };
            }

            GetUserByEmailDto response = new()
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                Email = currentUser.Email,
                Phone = currentUser.Phone,
                UserRole = currentUser.UserRole.ToString(),
                Favorites = currentUser.Favorites,
                UserCars = currentUser.UserCars,
                UserImagePath = currentUser.UserImagePath,

            };

            return new Result<GetUserByEmailDto>() { Data = response, Errors = [], IsSuccess = true };
        }
    }
}
