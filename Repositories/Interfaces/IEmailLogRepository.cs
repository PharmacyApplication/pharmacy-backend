using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface IEmailLogRepository
    {
        Task SaveLogAsync(EmailLog log);
        Task<IEnumerable<EmailLog>> GetAllLogsAsync();
        Task<IEnumerable<EmailLog>> GetLogsByOrderIdAsync(int orderId);
    }
}
