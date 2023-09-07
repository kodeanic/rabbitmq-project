using Infrastructure.IServices;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] OrderDto orderDto)
    {
        await _orderService.CreateOrder(orderDto.Email, orderDto.BookIds);
        return Ok();
    }
}