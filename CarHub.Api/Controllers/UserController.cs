using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Users.Handlers;
using System.Reflection;
using Application.Services;
using Application.CQRS.Favorites.Handlers;

namespace CarHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(ISender sender) : Controller
{
    private readonly ISender _sender = sender;

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromForm] Register.RegisterCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpPut("Update")]
    [AllowAnonymous]
    public async Task<IActionResult> Update([FromForm] Application.CQRS.Users.Handlers.Update.Command request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpDelete("Remove")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var request = new Application.CQRS.Users.Handlers.UserRemove.UserDeleteCommand { Id = id };
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetAll")]
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _sender.Send(new UserGetAll.GetAllUsersQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpGet("GetById")]
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromQuery] GetById.Query request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetUserByEmail")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] GetUserByEmail.GetUserByEmailCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetUserFavorites")]
    [AllowAnonymous]

    public async Task<IActionResult> GetUserFavorites([FromQuery] GetUserFavorites.GetUserFavoritesCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] Application.CQRS.Users.Handlers.Login.LoginRequest request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetUserCars")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserCars([FromQuery] GetUserCars.GetUserCarsQuery request)
    {
        return Ok(await _sender.Send(request));
    }
}
