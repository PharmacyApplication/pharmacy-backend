using PharmacyAPI.DTOs.Order;

namespace PharmacyAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> PlaceOrderAsync(int userId, string shippingAddress);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        Task<OrderDto> GetOrderByIdAsync(int userId, int orderId);
    }
}
