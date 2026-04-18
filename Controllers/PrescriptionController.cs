using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs.Prescription;
using PharmacyAPI.Services.Interfaces;
using System.Security.Claims;

namespace PharmacyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        // POST /api/prescription/upload
        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm] UploadPrescriptionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                var result = await _prescriptionService.UploadPrescriptionAsync(userId, dto.MedicineId, dto.File);
                return Ok(result);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/prescription/my
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyPrescriptions()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _prescriptionService.GetUserPrescriptionsAsync(userId);
            return Ok(result);
        }

        // PUT /api/prescription/{id}/review
        [HttpPut("{id}/review")]
        [Authorize(Roles = "Admin,Department")]
        public async Task<IActionResult> Review(int id, [FromBody] ReviewPrescriptionDto dto)
        {
            int reviewedBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                var result = await _prescriptionService.ReviewPrescriptionAsync(id, dto.Status, dto.RejectionReason, reviewedBy);
                return Ok(result);
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // Inline DTO for review — simple enough to not need its own file
    public class ReviewPrescriptionDto
    {
        public string Status { get; set; } = string.Empty; // Approved or Rejected
        public string? RejectionReason { get; set; }
    }
}
