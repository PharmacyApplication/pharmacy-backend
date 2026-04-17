using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs.Medicine;
using PharmacyAPI.Services.Interfaces;
using System.Security.Claims;

namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/medicine")]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        // GET /api/medicine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetAll()
        {
            var medicines = await _medicineService.GetAllMedicinesAsync();
            return Ok(medicines);
        }

        // GET /api/medicine/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineDto>> GetById(int id)
        {
            var medicine = await _medicineService.GetMedicineByIdAsync(id);
            if (medicine == null) return NotFound(new { message = "Medicine not found." });
            return Ok(medicine);
        }

        // GET /api/medicine/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetByCategory(int categoryId)
        {
            var medicines = await _medicineService.GetMedicinesByCategoryAsync(categoryId);
            return Ok(medicines);
        }

        // POST /api/medicine — Admin only
        [HttpPost]
      //  [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MedicineDto>> Create([FromBody] CreateMedicineDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _medicineService.CreateMedicineAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.MedicineId }, created);
        }

        // PUT /api/medicine/{id} — Admin only
        [HttpPut("{id}")]
      //  [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MedicineDto>> Update(int id, [FromBody] UpdateMedicineDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _medicineService.UpdateMedicineAsync(id, dto);
            if (updated == null) return NotFound(new { message = "Medicine not found." });
            return Ok(updated);
        }

        // DELETE /api/medicine/{id} — Admin only (soft delete)
        [HttpDelete("{id}")]
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var success = await _medicineService.DeleteMedicineAsync(id);
            if (!success) return NotFound(new { message = "Medicine not found." });
            return Ok(new { message = "Medicine deactivated successfully." });
        }

        // GET /api/medicine/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _medicineService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }

}
