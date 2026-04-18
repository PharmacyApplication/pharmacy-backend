using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs;
using PharmacyAPI.Repositories.Interfaces;

namespace PharmacyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IEmailLogRepository _emailLogRepo;

        public NotificationController(IEmailLogRepository emailLogRepo)
        {
            _emailLogRepo = emailLogRepo;
        }

        // GET /api/notification/logs
        [HttpGet("logs")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _emailLogRepo.GetAllLogsAsync();
            var result = logs.Select(l => new EmailLogDto
            {
                EmailLogId = l.EmailLogId,
                UserId = l.UserId,
                OrderId = l.OrderId,
                EmailType = l.EmailType,
                SentAt = l.SentAt,
                Status = l.Status,
                ErrorMessage = l.ErrorMessage
            });
            return Ok(result);
        }

        // GET /api/notification/logs/order/{orderId}
        [HttpGet("logs/order/{orderId:int}")]
        [Authorize]
        public async Task<IActionResult> GetLogsByOrder(int orderId)
        {
            var logs = await _emailLogRepo.GetLogsByOrderIdAsync(orderId);
            var result = logs.Select(l => new EmailLogDto
            {
                EmailLogId = l.EmailLogId,
                UserId = l.UserId,
                OrderId = l.OrderId,
                EmailType = l.EmailType,
                SentAt = l.SentAt,
                Status = l.Status,
                ErrorMessage = l.ErrorMessage
            });
            return Ok(result);
        }
    }
}
