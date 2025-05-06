using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Categories.Handlers;

namespace CarHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(ISender sender) : ControllerBase
{

    private readonly ISender _sender = sender;

    [HttpPost("Add")]
    public async Task<IActionResult> AddCategory(AddCategory.AddCommand request)
    {
        var result = await _sender.Send(request);
        return Ok(result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllCategory()
    {
        var result = await _sender.Send(new GetAllCategory.GetAllCategoryQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }

    [HttpDelete("Remove")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var request = new Remove.CategoryDeleteCommand { Id = id };
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        var request = new GetByIdCategory.CategoryGetByIdCommand { Id = id };
        var result = await _sender.Send(request);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Data);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateCategory.CategoryCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetCategoriesWithProducts")]
    public async Task<IActionResult> GetCategoriesWithProducts()
    {
       
        var result = await _sender.Send(new GetCategoriesWithProductsQuery());
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Data);
    }

}
