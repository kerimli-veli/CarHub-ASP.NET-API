using Application.CQRS.Users.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers
{
    public class GetUserFavorites
    {
        public class GetUserFavoritesCommand : IRequest<Result<List<GetUserFavoritesDto>>>
        {
            public int UserId { get; set; }
        }

        public sealed class Handler : IRequestHandler<GetUserFavoritesCommand, Result<List<GetUserFavoritesDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<List<GetUserFavoritesDto>>> Handle(GetUserFavoritesCommand request, CancellationToken cancellationToken)
            {
                var favoriteCars = await _unitOfWork.UserRepository.GetUserFavoritesAsync(request.UserId);

                if (favoriteCars == null || !favoriteCars.Any())
                {
                    return new Result<List<GetUserFavoritesDto>>
                    {
                        Data = null,
                        IsSuccess = false,
                        Errors = ["No favorite cars found for this user."]
                    };
                }

                var favoriteCarsDto = _mapper.Map<List<GetUserFavoritesDto>>(favoriteCars);

                return new Result<List<GetUserFavoritesDto>> { Data = favoriteCarsDto, IsSuccess = true };
            }
        }

    }
}

