using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs.Medicine;
using PharmacyAPI.Services.Interfaces;

namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET /api/inventory — Admin or Department
        [HttpGet]
        [Authorize(Roles = "Admin,Department")]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetAll()
        {
            var inventory = await _inventoryService.GetAllInventoryAsync();
            return Ok(inventory);
        }

        // PUT /api/inventory/{medicineId} — Admin only
        [HttpPut("{medicineId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<InventoryDto>> Update(int medicineId, [FromBody] UpdateInventoryDto dto)
        {
            var updated = await _inventoryService.UpdateInventoryAsync(medicineId, dto);
            if (updated == null) return NotFound(new { message = "Inventory record not found for the given medicine." });
            return Ok(updated);
        }

        // GET /api/inventory/low-stock — Admin only
        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetLowStock()
        {
            var lowStock = await _inventoryService.GetLowStockAsync();
            return Ok(lowStock);
        }
    }

}
