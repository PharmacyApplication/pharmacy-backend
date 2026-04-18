using Microsoft.AspNetCore.Authorization;
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

        // GET /api/medicine — Active medicines only (public/customer)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetAll()
        {
            var medicines = await _medicineService.GetAllMedicinesAsync();
            return Ok(medicines);
        }

        // GET /api/medicine/all — All medicines including inactive (admin)
        [HttpGet("all")]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetAllIncludingInactive()
        {
            var medicines = await _medicineService.GetAllMedicinesIncludingInactiveAsync();
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
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MedicineDto>> Create([FromBody] CreateMedicineDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _medicineService.CreateMedicineAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.MedicineId }, created);
        }

        // PUT /api/medicine/{id} — Admin only (also updates inventory)
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MedicineDto>> Update(int id, [FromBody] UpdateMedicineDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _medicineService.UpdateMedicineAsync(id, dto);
            if (updated == null) return NotFound(new { message = "Medicine not found." });
            return Ok(updated);
        }

        // DELETE /api/medicine/{id} — Soft delete (deactivate), reversible
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _medicineService.DeleteMedicineAsync(id);
            if (!success) return NotFound(new { message = "Medicine not found." });
            return Ok(new { message = "Medicine deactivated successfully." });
        }

        // DELETE /api/medicine/{id}/permanent — Hard delete, irreversible
        [HttpDelete("{id}/permanent")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var success = await _medicineService.HardDeleteMedicineAsync(id);
            if (!success) return NotFound(new { message = "Medicine not found." });
            return Ok(new { message = "Medicine permanently deleted." });
        }

        // PATCH /api/medicine/{id}/restore — Restore deactivated medicine
        [HttpPatch("{id}/restore")]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MedicineDto>> Restore(int id)
        {
            var restored = await _medicineService.RestoreMedicineAsync(id);
            if (restored == null) return NotFound(new { message = "Medicine not found." });
            return Ok(restored);
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