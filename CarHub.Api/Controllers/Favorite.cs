using Application.CQRS.Favorites.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CarHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class Favorite(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;


    [HttpGet("AddUserFavorites")]
    //[AllowAnonymous]
    public async Task<IActionResult> AddUserFavorites([FromQuery] AddFavoriteCar.AddFavoriteCarCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpDelete("RemoveFavoriteCar")]
    //[AllowAnonymous]
    public async Task<IActionResult> RemoveFavoriteCar([FromQuery] RemoveFavoriteCar.RemoveFavoriteCarCommand request)
    {
        return Ok(await _sender.Send(request));
    }
}
