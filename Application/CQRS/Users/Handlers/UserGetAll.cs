using Application.CQRS.Users.ResponseDtos;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class UserGetAll
{
    public record struct GetAllUsersQuery : IRequest<Result<List<UserGetAllDto>>> { }

    public sealed class Handler : IRequestHandler<GetAllUsersQuery, Result<List<UserGetAllDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<UserGetAllDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _unitOfWork.UserRepository.GetAll();
            if (users == null || !users.Any())
                return new Result<List<UserGetAllDto>>
                {
                    Data = [],
                    Errors = ["No users found"],
                    IsSuccess = false
                };

            var response = _mapper.Map<List<UserGetAllDto>>(users);

            return new Result<List<UserGetAllDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
