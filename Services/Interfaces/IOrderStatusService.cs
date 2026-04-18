using PharmacyAPI.DTOs;

namespace PharmacyAPI.Services.Interfaces
{
    public interface IOrderStatusService
    {
        Task<IEnumerable<object>> GetAllOrdersAsync();
        Task<object?> GetOrderByIdAsync(int orderId);
        Task<(bool Success, string Message)> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateDto dto);
    }
}
