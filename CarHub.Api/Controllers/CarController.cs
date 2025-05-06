using Application.CQRS.Cars.Handlers;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Cars.Handlers.CarGetById;

namespace CarHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarController(ISender sender) : Controller
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromForm] Application.CQRS.Cars.Handlers.CarAdd.CarAddCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpPut("CarUpdate")]
    public async Task<IActionResult> CarUpdate([FromForm] Application.CQRS.Cars.Handlers.CarUpdate.UpdateCarCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpDelete("Remove")]
    public async Task<IActionResult> Remove([FromQuery] int id)
    {
        var request = new Application.CQRS.Cars.Handlers.CarRemove.CarDeleteCommand { Id = id };
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllCars()
    {
        var result = await _sender.Send(new CarGetAll.GetAllCarsQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpGet("GetAllBodyTypes")]
    public async Task<IActionResult> GetAllBodyTypes()
    {
        var result = await _sender.Send(new GetAllBodyTypes.GetAllBodyTypesQuery());

        if(!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] CarGetByIdCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("CarFilter")]
    public async Task<IActionResult> FilterCars([FromQuery] GetFilteredCars.CarGetFilteredCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }


}
