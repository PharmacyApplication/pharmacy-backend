using PharmacyAPI.DTOs;
using PharmacyAPI.Repositories.Interfaces;
using PharmacyAPI.Services.Interfaces;

namespace PharmacyAPI.Services
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepository _orderStatusRepo;
        private readonly IEmailService _emailService;

        private static readonly Dictionary<string, List<string>> ValidTransitions = new()
        {
            { "Ordered",   new List<string> { "Shipped", "Cancelled" } },
            { "Shipped",   new List<string> { "Delivered", "Cancelled" } },
            { "Delivered", new List<string>() },
            { "Cancelled", new List<string>() }
        };

        public OrderStatusService(IOrderStatusRepository orderStatusRepo, IEmailService emailService)
        {
            _orderStatusRepo = orderStatusRepo;
            _emailService = emailService;
        }

        public async Task<IEnumerable<object>> GetAllOrdersAsync()
        {
            var orders = await _orderStatusRepo.GetAllOrdersWithUsersAsync();
            return orders.Select(o => new
            {
                o.OrderId,
                CustomerName = o.User?.FullName,
                CustomerEmail = o.User?.Email,
                o.TotalAmount,
                o.Status,
                o.PlacedAt,
                o.UpdatedAt
            });
        }

        public async Task<object?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderStatusRepo.GetOrderByIdAsync(orderId);
            if (order == null) return null;

            return new
            {
                order.OrderId,
                order.Status,
                order.PlacedAt,
                order.UpdatedAt,
                order.ShippingAddress,
                order.TotalAmount,
                CustomerName = order.User?.FullName
            };
        }

        public async Task<(bool Success, string Message)> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateDto dto)
        {
            var order = await _orderStatusRepo.GetOrderWithDetailsAsync(orderId);
            if (order == null)
                return (false, "Order not found.");

            var newStatus = dto.Status;

            if (!ValidTransitions.ContainsKey(order.Status))
                return (false, $"Unknown current status: {order.Status}");

            if (!ValidTransitions[order.Status].Contains(newStatus))
                return (false, $"Invalid transition from '{order.Status}' to '{newStatus}'.");

            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderStatusRepo.UpdateOrderAsync(order);

            if (order.User != null)
            {
                _ = newStatus switch
                {
                    "Shipped" => _emailService.SendShippedNotificationAsync(order.User, order),
                    "Delivered" => _emailService.SendDeliveredNotificationAsync(order.User, order),
                    "Cancelled" => _emailService.SendOrderConfirmationAsync(order.User, order),
                    _ => Task.CompletedTask
                };
            }

            return (true, $"Order status updated to '{newStatus}'.");
        }
    }
}
