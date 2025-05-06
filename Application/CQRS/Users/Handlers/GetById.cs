using Application.CQRS.Users.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class GetById
{
    public class Query : IRequest<Result<GetByIdDto>>
    {
        public int Id { get; set; }
    }

    public sealed class Handler : IRequestHandler<Query, Result<GetByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetByIdDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);

            if (currentUser == null)
            {
                return new Result<GetByIdDto>() { Errors = ["User not found"], IsSuccess = false };
            }

            GetByIdDto response = new()
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                Email = currentUser.Email,
                Phone = currentUser.Phone,
                UserRole = currentUser.UserRole.ToString(),
                FavoriteCarIds = currentUser.Favorites.Select(f => f.CarId).ToList(),
                //UserCars = currentUser.UserCars,
                UserImagePath = currentUser.UserImagePath,
            
            };

            return new Result<GetByIdDto>() { Data = response, Errors = [], IsSuccess = true };
        }
    }
}
