using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.DTOs.Order;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;
using PharmacyAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IInventoryRepository _inventoryRepository; // Injected from Module 2
     //   private readonly IEmailService _emailService;              // Injected from Module 4 — not implemented here
        private readonly AppDbContext _context;                    // Used to load user details for email

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IInventoryRepository inventoryRepository,
         //   IEmailService emailService,
            AppDbContext context)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _inventoryRepository = inventoryRepository;
           // _emailService = emailService;
            _context = context;
        }

        public async Task<OrderDto> PlaceOrderAsync(int userId, string shippingAddress)
        {
            // Step 1: Get cart and items
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty.");

            var cartItems = cart.CartItems.ToList();

            // Step 2: Create the Order record
            decimal total = cartItems.Sum(ci => (ci.Medicine?.Price ?? 0) * ci.Quantity);
            var order = new Order
            {
                UserId = userId,
                TotalAmount = total,
                Status = "Ordered",
                ShippingAddress = shippingAddress,
                PlacedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Step 3: Create OrderItem records + Step 4: Deduct inventory
            foreach (var ci in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = createdOrder.OrderId,
                    MedicineId = ci.MedicineId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Medicine?.Price ?? 0,
                    PrescriptionId = ci.PrescriptionId
                };
                await _orderRepository.AddOrderItemAsync(orderItem);

                // Deduct inventory via Module 2's IInventoryRepository
                var inventory = await _inventoryRepository.GetByMedicineIdAsync(ci.MedicineId);
                if (inventory != null)
                {
                    inventory.QuantityInStock = Math.Max(0, inventory.QuantityInStock - ci.Quantity);
                    inventory.LastUpdated = DateTime.UtcNow;
                    await _inventoryRepository.UpdateAsync(inventory);
                }
            }

            // Step 5: Clear cart items
            await _cartRepository.ClearCartItemsAsync(cart.CartId);

            // Step 6: Send order confirmation email (Module 4 implements this)
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
              //  await _emailService.SendOrderConfirmationAsync(user, createdOrder);
            }

            // Return full order DTO
            return await GetOrderByIdAsync(userId, createdOrder.OrderId);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapToDto);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int userId, int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order {orderId} not found.");
            if (order.UserId != userId)
                throw new UnauthorizedAccessException("Access denied.");
            return MapToDto(order);
        }

        private static OrderDto MapToDto(Order o) => new OrderDto
        {
            OrderId = o.OrderId,
            UserId = o.UserId,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            ShippingAddress = o.ShippingAddress,
            PlacedAt = o.PlacedAt,
            UpdatedAt = (DateTime)o.UpdatedAt,
            Items = o.OrderItems?.Select(oi => new OrderItemDto
            {
                OrderItemId = oi.OrderItemId,
                MedicineId = oi.MedicineId,
                MedicineName = oi.Medicine?.Name ?? string.Empty,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                PrescriptionId = oi.PrescriptionId
            }).ToList() ?? new()
        };
    }
}