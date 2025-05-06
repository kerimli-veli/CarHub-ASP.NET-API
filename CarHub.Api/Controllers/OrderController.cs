using Application.CQRS.Order.Commands;
using Application.CQRS.Order.Handlers;
using Application.CQRS.Order.ResponseDtos;
using Common.GlobalResponses.Generics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrder.CreateOrderCommand command)
        {
            var result = await _sender.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Errors = result.Errors });

            return Ok(result.Data);
        }

        [HttpGet("GetOrdersByUserId")]
        public async Task<IActionResult> GetOrdersByUserId([FromQuery] int userId)
        {
            var query = new GetOrdersByUserId.GetOrdersByUserIdQuery
            {
                UserId = userId
            };

            Result<List<OrderDto>> result = await _sender.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var result = await _sender.Send(new GetOrderById.GetOrderByIdQuery
            {
                OrderId = orderId
            });

            if (!result.IsSuccess)
            {
                return NotFound(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _sender.Send(new GetAllOrders.GetAllOrdersQuery());

            if (!result.IsSuccess)
            {
                return NotFound(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var request = new Application.CQRS.Order.Handlers.DeleteOrder.OrderDeleteCommand { Id = id };
            return Ok(await _sender.Send(request));
        }

        [HttpPut("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusCommand command)
        {
            var result = await _sender.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
