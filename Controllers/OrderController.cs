using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs.Order;
using PharmacyAPI.Services.Interfaces;
using System.Security.Claims;

namespace PharmacyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST /api/order/place
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                var order = await _orderService.PlaceOrderAsync(userId, dto.ShippingAddress);
                return Ok(order);
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/order/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        // GET /api/order/{orderId}
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                var order = await _orderService.GetOrderByIdAsync(userId, orderId);
                return Ok(order);
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (System.UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
    }
