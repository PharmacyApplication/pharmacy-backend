using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs;
using PharmacyAPI.Services.Interfaces;

namespace PharmacyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatusService;

        public OrderStatusController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        // GET /api/orderstatus/all
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Department")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderStatusService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // GET /api/orderstatus/{orderId}
        [HttpGet("{orderId:int}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderStatusService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound(new { message = "Order not found." });

            return Ok(order);
        }

        // PUT /api/orderstatus/{orderId}
        [HttpPut("{orderId:int}")]
        [Authorize(Roles = "Admin,Department")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest(new { message = "Status is required." });

            var (success, message) = await _orderStatusService.UpdateOrderStatusAsync(orderId, dto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }
    }
}
