using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public class IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId);
    }
}
