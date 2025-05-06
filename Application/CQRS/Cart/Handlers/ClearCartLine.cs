using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Carts.Handlers;

public sealed class ClearCartLineCommand : IRequest<Result<bool>>
{
    public int CartId { get; set; }

    public ClearCartLineCommand(int cartId)
    {
        CartId = cartId;
    }
}
public class ClearCartLineHandler(IUnitOfWork unitOfWork) : IRequestHandler<ClearCartLineCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> Handle(ClearCartLineCommand request, CancellationToken cancellationToken)
    {
        
        var cart = await _unitOfWork.CartRepository.GetCartWithLinesAsync(request.CartId);
        if (cart == null)
        {
            return new Result<bool> { Errors = { "Cart not found" }, IsSuccess = false };
        }
        cart.CartLines.Clear();
        await _unitOfWork.SaveChangeAsync();

        return new Result<bool> { IsSuccess = true };
    }
}
