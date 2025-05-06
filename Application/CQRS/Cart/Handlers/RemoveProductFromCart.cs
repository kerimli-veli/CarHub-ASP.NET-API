using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using System.Threading;
using System.Threading.Tasks;

public class RemoveProductFromCart
{
    
    public class RemoveProductFromCartCommand : IRequest<Result<Unit>>
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
    }

 
    public sealed class Handler : IRequestHandler<RemoveProductFromCartCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.CartRepository.RemoveProductFromCartAsync(request.CartId, request.ProductId);
                await _unitOfWork.SaveChangeAsync();

                return new Result<Unit>
                {
                    IsSuccess = true,
                    Data = Unit.Value
                };
            }
            catch (Exception ex)
            {
                return new Result<Unit>
                {
                    IsSuccess = false,
                    
                    Errors = new List<string> { "An error occurred while removing the product:" }
                };
            }
        }
    }
}