using PharmacyAPI.Models;

namespace PharmacyAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(User user, Order order);
        Task SendShippedNotificationAsync(User user, Order order);
        Task SendDeliveredNotificationAsync(User user, Order order);

    }
}
