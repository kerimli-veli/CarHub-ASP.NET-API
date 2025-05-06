using Application.CQRS.Users.ResponseDtos;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;
using static Application.CQRS.Favorites.Handlers.AddFavoriteCar;

namespace Application.CQRS.Favorites.Handlers;

public class AddFavoriteCar
{
    public class AddFavoriteCarCommand : IRequest<Result<Unit>>
    {
        public int UserId { get; set; }
        public int CarId { get; set; }
    }

    public sealed class Handler : IRequestHandler<AddFavoriteCarCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(AddFavoriteCarCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            var currentCar = await _unitOfWork.CarRepository.GetByIdAsync(request.CarId);

            if (currentUser == null || currentCar == null)
            {
                throw new BadRequestException("User or Car not found");
            }

            var existingFavorite = currentUser.Favorites.FirstOrDefault(c => c.CarId == request.CarId);


            await _unitOfWork.FavoriteRepository.AddFavoriteCarAsync(request.UserId, request.CarId);
            await _unitOfWork.SaveChangeAsync();

            return new Result<Unit> { Errors = [], IsSuccess = true };
        }
    }

}
