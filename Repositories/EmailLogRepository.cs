using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;

namespace PharmacyAPI.Repositories
{
    public class EmailLogRepository : IEmailLogRepository
    {
        private readonly AppDbContext _context;

        public EmailLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveLogAsync(EmailLog log)
        {
            _context.EmailLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmailLog>> GetAllLogsAsync()
        {
            return await _context.EmailLogs
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmailLog>> GetLogsByOrderIdAsync(int orderId)
        {
            return await _context.EmailLogs
                .Where(e => e.OrderId == orderId)
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
        }
    }
}
