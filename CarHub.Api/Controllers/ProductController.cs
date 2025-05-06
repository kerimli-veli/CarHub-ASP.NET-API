using Application.CQRS.Products.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Products.Handlers.AddProduct;

namespace CarHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] AddProduct.AddProductCommand request)
    {
        var result = await _sender.Send(request);
        return Ok(result);
    }

    [HttpDelete("DeleteProduct")]
    public async Task<IActionResult> DeleteProduct([FromBody] DeleteProduct.DeleteCommand request)
    {
        var result = await _sender.Send(request);
        return Ok(result);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetByIdProduct([FromQuery] int id)
    {
        var request = new GetByIdProduct.ProductGetByIdCommand { Id = id };
        var result = await _sender.Send(request);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Data);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllCategory()
    {
        var result = await _sender.Send(new GetAll.GetAllProductQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Data);
    }
    [HttpPut("UpdateProduct")]
    public async Task<IActionResult> Update([FromBody] UpdateProduct.UpdateProductCommand request)
    {
        return Ok(await _sender.Send(request));
    }

    [HttpGet("GetProductsByCategoryId")]
    public async Task<ActionResult> GetProductsByCategoryId(int categoryId)
    { 
        var query = new ProductResponse.GetProductsByCategoryIdQuery(categoryId);
        var result = await _sender.Send(query);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("GetByNameProduct")]
    public async Task<IActionResult> GetByNameProduct([FromQuery] string name)
    {
        var request = new GetByNameProduct.ProductGetByNameQuery { Name = name };
        var result = await _sender.Send(request);
        if (!result.IsSuccess)
        {
            return NotFound(result.Errors);
        }
        return Ok(result.Data);
    }

    [HttpGet("GetProductsByPriceRange")]
    public async Task<IActionResult> GetProductsByPriceRange([FromQuery] decimal maxPrice)
    {
        var query = new GetProductsByPriceRange.ProductRangeCommand(maxPrice);
        var products = await _sender.Send(query);
        return Ok(products);
    }

}


