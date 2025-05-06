using Application.CQRS.Auctions.Handler;
using Application.CQRS.Auctions.Handlers;
using Application.CQRS.Cars.Handlers;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Cars.Handlers.CarGetById;

namespace CarHub.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuctionController(ISender sender) : Controller
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuction.CreateAuctionCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("AuctionGetById")]
    public async Task<IActionResult> AuctionGetById([FromQuery] GetByIdAsync.GetByIdAuctionCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("AuctionGetBySellerId")]
    public async Task<IActionResult> AuctionGetBySellerId([FromQuery] GetBySellerIdAsync.GetBySellerIdCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("AuctionsGetAllActive")]
    public async Task<IActionResult> AuctionGetAllActive()
    {
        var result = await _sender.Send(new GetAllActiveAsync.GetAllActiveCommand());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpGet("GetAllAuctions")]
    public async Task<IActionResult> GetAllAuctions()
    {
        var result = await _sender.Send(new GetAllAuctions.GetAllAuctionsCommand());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpDelete("DeleteAuction")]
    public async Task<IActionResult> DeleteAuction([FromQuery] int id)
    {
        var request = new Application.CQRS.Auctions.Handler.AuctionDelete.DeleteAuctionCommand { Id = id };
        return Ok(await _sender.Send(request));
    }

    [HttpPut("SetIsActive")]
    public async Task<IActionResult> SetIsActive([FromQuery] SetIsActiveAsync.SetIsActiveCommand request)
    {
        return Ok(await _sender.Send(request));
    }

}
