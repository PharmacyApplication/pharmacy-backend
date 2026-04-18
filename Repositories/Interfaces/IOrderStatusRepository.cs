using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface IOrderStatusRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersWithUsersAsync();
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
    }
}
