
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;


public class UserRemove
{
    public class UserDeleteCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<UserDeleteCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.Remove(request.Id);
            await _unitOfWork.SaveChangeAsync();
            return new Result<Unit> { Errors = [], IsSuccess = true };
        }
    }
}