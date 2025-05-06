using Common.Exceptions;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using static Application.CQRS.Favorites.Handlers.RemoveFavoriteCar;

namespace Application.CQRS.Favorites.Handlers;

public class RemoveFavoriteCar
{
    public class RemoveFavoriteCarCommand : IRequest<Result<Unit>>
    {
        public int UserId { get; set; }
        public int CarId { get; set; }
    }

    public class Handler : IRequestHandler<RemoveFavoriteCarCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(RemoveFavoriteCarCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            var currentCar = await _unitOfWork.CarRepository.GetByIdAsync(request.CarId);

            if (currentUser == null || currentCar == null)
            {
                throw new BadRequestException("User or Car not found");
            }

            var existingFavorite = currentUser.Favorites.FirstOrDefault(c => c.CarId == request.CarId);

            if (existingFavorite == null)
            {
                throw new BadRequestException("Car is not in the favorites.");
            }

            await _unitOfWork.FavoriteRepository.RemoveFavoriteCarAsync(request.UserId, request.CarId);
            await _unitOfWork.SaveChangeAsync();

            return new Result<Unit> { Errors = [], IsSuccess = true };
        }
    }
}
