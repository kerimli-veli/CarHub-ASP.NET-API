using Application.CQRS.Cart.Handlers;
using Application.CQRS.Cart.Queries;
using Application.CQRS.Carts.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> AddCart([FromBody] Application.CQRS.Cart.Handlers.AddCart.AddCartCommand request)
    {
        var result = await _sender.Send(request);
        return Ok(result);
    }
    [HttpPost("AddProductToCart")]
    public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCart.AddProductToCartCommand request)
    {
        var result = await _sender.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete("ClearCartLines")]
    public async Task<IActionResult> ClearCartLines([FromBody] ClearCartLineCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Cart lines cleared successfully" });
        }

        return BadRequest(new { errors = result.Errors });
    }

    [HttpGet("GetCartWithLinesByUserId")]
    public async Task<IActionResult> GetCartWithLinesByUserId([FromQuery] int userId)
    {
        var request = new GetCartWithLinesByUserId.GetCartWithLinesByUserIdQuery { UserId = userId };
        var result = await _sender.Send(request);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Data);
    }

    [HttpGet("GetCartWithLines")]
    public async Task<IActionResult> GetCartWithLines([FromQuery] int cartId)
    {
        var request = new GetCartWithLines.GetCartWithLinesQuery { CartId = cartId };
        var result = await _sender.Send(request);

        if (!result.IsSuccess)
            return NotFound(result.Errors);

        return Ok(result.Data);
    }
    [HttpDelete("RemoveProductFromCart")]
    public async Task<IActionResult> RemoveProductFromCart([FromBody] RemoveProductFromCart.RemoveProductFromCartCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Product removed from cart successfully" });
        }

        return BadRequest(result);
    }

    [HttpPut("UpdateProductQuantityInCart")]
    public async Task<IActionResult> UpdateProductQuantity([FromBody] UpdateProductQuantityInCart.UpdateProductQuantityInCartCommand request)
    {
        var result = await _sender.Send(request);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("GetCartTotalPrice")]
    public async Task<IActionResult> GetCartTotalPrice([FromQuery] int cartId)
    {
        var request = new GetCartTotalPrice.GetCartTotalPriceQuery(cartId);
        var result = await _sender.Send(request);

        if (!result.IsSuccess)
            return NotFound(result.Errors);

        return Ok(result.Data);
    }
}